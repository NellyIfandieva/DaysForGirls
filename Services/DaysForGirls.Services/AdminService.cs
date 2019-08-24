using DaysForGirls.Data;
using DaysForGirls.Data.Models;
using DaysForGirls.Services.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaysForGirls.Services
{
    public class AdminService : IAdminService
    {
        private readonly DaysForGirlsDbContext db;
        private readonly IPictureService pictureService;
        private readonly ICustomerReviewService customerReviewService;

        public AdminService(
            DaysForGirlsDbContext db,
            IPictureService pictureService,
            ICustomerReviewService customerReviewService)
        {
            this.db = db;
            this.pictureService = pictureService;
            this.customerReviewService = customerReviewService;
        }

        public async Task<int> CreateAsync(ProductServiceModel productServiceModel)
        {
            ProductType productTypeInDb = db.ProductTypes
                .SingleOrDefault(pT => pT.Name == productServiceModel.ProductType.Name);

            if(productTypeInDb == null)
            {
                throw new ArgumentNullException(nameof(productTypeInDb));
            }

            Category categoryInDb = this.db.Categories
                .SingleOrDefault(cat => cat.Name == productServiceModel.Category.Name);

            if(categoryInDb == null)
            {
                throw new ArgumentNullException(nameof(categoryInDb));
            }

            Manufacturer manufacturerInDb = this.db.Manufacturers
                .SingleOrDefault(man => man.Name == productServiceModel.Manufacturer.Name);

            if(manufacturerInDb == null)
            {
                throw new ArgumentNullException(nameof(manufacturerInDb));
            }

            Quantity quantityOfProduct = new Quantity
            {
                AvailableItems = productServiceModel.Quantity.AvailableItems
            };

            this.db.Quantities.Add(quantityOfProduct);

            int result = await this.db.SaveChangesAsync();
            bool quantityIsAdded = result > 0;

            Product product = new Product
            {
                Name = productServiceModel.Name,
                ProductType = productTypeInDb,
                Category = categoryInDb,
                Description = productServiceModel.Description,
                Colour = productServiceModel.Colour,
                Size = productServiceModel.Size,
                Price = productServiceModel.Price,
                Manufacturer = manufacturerInDb,
                Reviews = new List<CustomerReview>(),
                Pictures = new List<Picture>()
            };

            if(productServiceModel.SaleId != null)
            {
                product.SaleId = productServiceModel.SaleId;
            }

            if(quantityIsAdded)
            {
                product.Quantity = quantityOfProduct;
            }

            product.Pictures = productServiceModel.Pictures
                .Select(p => new Picture
                {
                    PictureUrl = p.PictureUrl
                })
                .ToList();

            this.db.Products.Add(product);
            await db.SaveChangesAsync();

            int productId = product.Id;

            return productId;
        }

        public IQueryable<AdminProductAllServiceModel> DisplayAll()
        {
            var allProducts = this.db.Products
                .Where(p => p.IsDeleted == false)
                .Select(p => new AdminProductAllServiceModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Category = new CategoryServiceModel
                    {
                        Name = p.Category.Name
                    },
                    Picture = new PictureServiceModel
                    {
                        PictureUrl = p.Pictures.ElementAt(0).PictureUrl
                    },
                    Price = p.Price,
                    AvailableItems = p.Quantity.AvailableItems,
                    Manufacturer = new ManufacturerServiceModel
                    {
                        Name = p.Manufacturer.Name
                    },
                    IsDeleted = p.IsDeleted,
                    IsInSale = p.IsInSale,
                    SaleId = p.SaleId,
                    ShoppingCartId = p.ShoppingCartId,
                    OrderId = p.OrderId
                });

            return allProducts;
        }

        public async Task<ProductServiceModel> GetProductByIdAsync(int productId)
        {
            var product = await this.db.Products
                .Include(p => p.Category)
                .Include(p => p.ProductType)
                .Include(p => p.Manufacturer)
                .Include(p => p.Quantity)
                .Include(p => p.Sale)
                .Include(p => p.ShoppingCart)
                .SingleOrDefaultAsync(p => p.Id == productId);

            var productPictures = await this.pictureService
                .GetPicturesOfProductByProductId(product.Id).ToListAsync();

            var productReviews = await this.customerReviewService
                .GetAllCommentsOfProductByProductId(product.Id)
                .ToListAsync();

            ProductServiceModel productToReturn = new ProductServiceModel
            {
                Id = product.Id,
                Name = product.Name,
                ProductType = new ProductTypeServiceModel
                {
                    Name = product.ProductType.Name
                },
                Category = new CategoryServiceModel
                {
                    Name = product.Category.Name
                },
                Description = product.Description,
                Pictures = productPictures,
                Colour = product.Colour,
                Size = product.Size,
                Price = product.Price,
                Manufacturer = new ManufacturerServiceModel
                {
                    Id = product.Manufacturer.Id,
                    Name = product.Manufacturer.Name
                },
                Quantity = new QuantityServiceModel
                {
                    AvailableItems = product.Quantity.AvailableItems
                },
                Reviews = productReviews,
                IsDeleted = product.IsDeleted,
                SaleId = product.SaleId,
                ShoppingCartId = product.ShoppingCartId
            };

            return productToReturn;
        }

        public async Task<bool> DeleteProductByIdAsync(int id)
        {
            var productToDelete = this.db.Products
                .SingleOrDefault(product => product.Id == id);

            productToDelete.IsDeleted = true;

            bool picturesAreDeleted = await this.pictureService
                .DeletePicturesOfDeletedProductAsync(productToDelete.Id);

            this.db.Update(productToDelete);
            int result = await this.db.SaveChangesAsync();
            bool productIsDeleted = result > 0;

            return productIsDeleted;
        }

        public async Task<bool> EditAsync(ProductServiceModel model)
        {
            ProductType productTypeOfProduct = await this.db.ProductTypes
                .SingleOrDefaultAsync(pT => pT.Name == model.ProductType.Name);

            Category categoryOfProduct = await this.db.Categories
                .SingleOrDefaultAsync(c => c.Name == model.Category.Name);

            Manufacturer manufacturerOfProduct = await this.db.Manufacturers
                .SingleOrDefaultAsync(m => m.Name == model.Manufacturer.Name);

            if (productTypeOfProduct == null
                || categoryOfProduct == null
                || manufacturerOfProduct == null)
            {
                throw new ArgumentNullException(nameof(productTypeOfProduct));
            }

            Product productInDb = await this.db.Products
                .Include(p => p.Category)
                .Include(p => p.ProductType)
                .Include(p => p.Manufacturer)
                .Include(p => p.Quantity)
                .Include(p => p.Sale)
                .SingleOrDefaultAsync(p => p.Id == model.Id);

            productInDb.Name = model.Name;
            productInDb.Description = model.Description;
            productInDb.Colour = model.Colour;
            productInDb.Size = model.Size;
            productInDb.Price = model.Price;
            productInDb.ManufacturerId = manufacturerOfProduct.Id;
            productInDb.Manufacturer = manufacturerOfProduct;
            productInDb.ProductTypeId = productTypeOfProduct.Id;
            productInDb.ProductType = productTypeOfProduct;
            productInDb.CategoryId = categoryOfProduct.Id;
            productInDb.Category = categoryOfProduct;
            productInDb.Quantity.AvailableItems = model.Quantity.AvailableItems;
            productInDb.SaleId = model.SaleId;

            productInDb.Pictures.Clear();

            foreach(var pic in model.Pictures)
            {
                Picture picture = new Picture
                {
                    PictureUrl = pic.PictureUrl,
                    ProductId = productInDb.Id,
                    Product = productInDb,
                };

                productInDb.Pictures.Add(picture);
            }

            this.db.Update(productInDb);
            int result = await db.SaveChangesAsync();

            bool productIsEdited = result > 0;

            return productIsEdited;
        }

        public async Task<bool> UploadNewPictureToProduct(int productId, string imageUrl)
        {
            var productInDb = this.db.Products.
                SingleOrDefault(p => p.Id == productId);

            Picture newPicture = new Picture
            {
                PictureUrl = imageUrl
            };

            productInDb.Pictures.Add(newPicture);
            this.db.Update(productInDb);
            int result = await this.db.SaveChangesAsync();

            bool pictureIsAdded = result > 0;

            return pictureIsAdded;
        }

        public async Task<bool> AddProductToSaleAsync(int productId, string saleId)
        { 
            var product = this.db.Products
                .SingleOrDefault(p => p.Id == productId);

            product.SaleId = saleId;
            product.IsInSale = true;

            this.db.Products.Update(product);
            int result = await this.db.SaveChangesAsync();
            bool productIsAddedToSale = result > 0;

            return productIsAddedToSale;
        }

        public async Task<bool> UploadNewPictureToProductAsync(int productId, string imageUrl)
        {
            var productInDb = await this.db.Products
                .SingleOrDefaultAsync(p => p.Id == productId);

            Picture newPicture = new Picture
            {
                PictureUrl = imageUrl
            };

            productInDb.Pictures.Add(newPicture);
            this.db.Update(productInDb);
            int result = await this.db.SaveChangesAsync();

            bool pictureIsAdded = result > 0;

            return pictureIsAdded;
        }

        public async Task<ProductServiceModel> GetProductByNameAsync(string productName)
        {
            var productWithName = await this.db.Products
                .Include(p => p.Category)
                .Include(p => p.ProductType)
                .Include(p => p.Manufacturer)
                .Include(p => p.Quantity)
                .SingleOrDefaultAsync(p => p.Name == productName);

            var pictures = await this.pictureService
                .GetPicturesOfProductByProductId(productWithName.Id).ToListAsync();

            var productReviews = await this.db.CustomerReviews
                .Where(cR => cR.ProductId == productWithName.Id)
                .Select(pR => new CustomerReviewServiceModel
                {
                    Id = pR.Id,
                    Title = pR.Title,
                    Text = pR.Text,
                    CreatedOn = pR.CreatedOn.ToString("dddd, dd MMMM yyyy"),
                    AuthorUsername = pR.Author.UserName
                }).ToListAsync();

            ProductServiceModel productToReturn = new ProductServiceModel
            {
                Id = productWithName.Id,
                Name = productWithName.Name,
                ProductType = new ProductTypeServiceModel
                {
                    Name = productWithName.ProductType.Name
                },
                Category = new CategoryServiceModel
                {
                    Name = productWithName.Category.Name
                },
                Description = productWithName.Description,
                Pictures = pictures,
                Colour = productWithName.Colour,
                Size = productWithName.Size,
                Price = productWithName.Price,
                Manufacturer = new ManufacturerServiceModel
                {
                    Name = productWithName.Manufacturer.Name
                },
                Quantity = new QuantityServiceModel
                {
                    AvailableItems = productWithName.Quantity.AvailableItems
                },
                Reviews = productReviews,
                IsDeleted = productWithName.IsDeleted,
                SaleId = productWithName.Sale.Id
            };

            return productToReturn;
        }

        private IQueryable<Product> GetAllProductsByIds(List<int> productIds)
        {
            var allSearchedProducts = this.db.Products
                .Where(p => productIds.Contains(p.Id)
                && p.IsDeleted == false);

            return allSearchedProducts;
        }

        public async Task<bool> SetProductsCartIdToNullAsync(List<int> productIds)
        {
            var products = await GetAllProductsByIds(productIds)
                .Include(p => p.Quantity)
                .ToListAsync();

            foreach(var product in products)
            {
                product.Quantity.AvailableItems++;
                product.ShoppingCartId = null;
            }

            this.db.UpdateRange(products);
            int result = await this.db.SaveChangesAsync();

            bool productsCartIdIsSetToNull = result > 0;

            return productsCartIdIsSetToNull;
        }

        public async Task<bool> SetOrderIdToProductsAsync(List<int> productIds, string orderId)
        {
            var productsToAddToOrder = await this.db.Products
                .Where(p => productIds.Contains(p.Id)).ToListAsync();

            foreach(var product in productsToAddToOrder)
            {
                product.OrderId = orderId;
                product.ShoppingCartId = null;
            }

            this.db.UpdateRange(productsToAddToOrder);
            int result = await this.db.SaveChangesAsync();

            bool productsAreAddedToOrder = result > 0;

            return productsAreAddedToOrder;
        }

        //public async Task<bool> AddProductsToSaleAsync(List<int> productIds, int saleId)
        //{
        //    var productsToAddToSale = await this.db.Products
        //        .Where(p => productIds.Contains(p.Id)
        //        && p.IsDeleted == false)
        //        .ToListAsync();

        //    foreach(var product in productsToAddToSale)
        //    {
        //        product.IsInSale = true;
        //        product.Sales.Add(new ProductSale
        //        {
        //            ProductId = product.Id,
        //            SaleId = saleId
        //        });

        //        this.db.Update(product);
        //    }

        //    int result = await this.db.SaveChangesAsync();

        //    bool productsAddedToSale = result > 0;

        //    return productsAddedToSale;
        //}

    }
}
