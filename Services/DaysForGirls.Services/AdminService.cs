namespace DaysForGirls.Services
{
    using DaysForGirls.Data;
    using DaysForGirls.Data.Models;
    using DaysForGirls.Services.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

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

            if (productTypeInDb == null)
            {
                return 0;
            }

            Category categoryInDb = this.db.Categories
                .SingleOrDefault(cat => cat.Name == productServiceModel.Category.Name);

            if (categoryInDb == null)
            {
                return 0;
            }

            Manufacturer manufacturerInDb = this.db.Manufacturers
                .SingleOrDefault(man => man.Name == productServiceModel.Manufacturer.Name);

            if (manufacturerInDb == null)
            {
                return 0;
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

            if (productServiceModel.SaleId != null)
            {
                product.SaleId = productServiceModel.SaleId;
            }

            if (quantityIsAdded)
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
                    SalePrice = p.SalePrice,
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

            if (product == null)
            {
                return null;
            }

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
                SalePrice = product.SalePrice,
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
                ShoppingCartId = product.ShoppingCartId,
                OrderId = product.OrderId
            };

            return productToReturn;
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
                return false;
            }

            Product productInDb = await this.db.Products
                .Include(p => p.Category)
                .Include(p => p.ProductType)
                .Include(p => p.Manufacturer)
                .Include(p => p.Quantity)
                .Include(p => p.Sale)
                .SingleOrDefaultAsync(p => p.Id == model.Id);

            if(productInDb == null)
            {
                return false;
            }

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

            if(productInDb.Pictures.Count > 0)
            {
                productInDb.Pictures.Clear();
            }

            foreach (var pic in model.Pictures)
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

        public async Task<bool> AddProductToSaleAsync(int productId, string saleId)
        {
            var product = await this.db.Products
                .SingleOrDefaultAsync(p => p.Id == productId);

            var sale = await this.db.Sales
                .SingleOrDefaultAsync(s => s.Id == saleId);

            if (product == null || sale == null)
            {
                return false;
            }

            product.SaleId = sale.Id;
            product.IsInSale = true;

            this.db.Products.Update(product);
            int result = await this.db.SaveChangesAsync();
            bool productIsAddedToSale = result > 0;

            return productIsAddedToSale;
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

            if(products.Count() < 1)
            {
                return true;
            }

            foreach (var product in products)
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
            if(productIds.Count() < 1 || orderId == null)
            {
                return false;
            }

            var productsToAddToOrder = await this.db.Products
                .Where(p => productIds.Contains(p.Id)).ToListAsync();

            if (productsToAddToOrder.Count() < 1)
            {
                return false;
            }

            foreach (var product in productsToAddToOrder)
            {
                product.OrderId = orderId;
                product.ShoppingCartId = null;
            }

            this.db.UpdateRange(productsToAddToOrder);
            int result = await this.db.SaveChangesAsync();

            bool productsAreAddedToOrder = result > 0;

            return productsAreAddedToOrder;
        }

        public async Task<string> EraseFromDb(int productId)
        {
            var productInDb = await this.db.Products
                .SingleOrDefaultAsync(p => p.Id == productId);

            string outcome = null;

            if (productInDb == null)
            {
                return outcome;
            }

            if (productInDb.ShoppingCartId != null || productInDb.OrderId != null)
            {
                if (productInDb.ShoppingCartId != null)
                {
                    outcome = productInDb.Name + "-" + "is in a Shopping Cart and cannot be deleted.";
                }
                else
                {
                    productInDb.IsDeleted = true;
                    this.db.Update(productInDb);
                    await this.db.SaveChangesAsync();
                    outcome = productInDb.Name + "-" + "has been purchased and was only set to IsDeleted.";
                }

                return outcome;
            }

            string productName = productInDb.Name;

            string saleId = productInDb.SaleId;

            if (saleId != null)
            {
                var sale = await this.db.Sales
                    .Include(s => s.Products)
                    .SingleOrDefaultAsync(s => s.Id == saleId);

                sale.Products.Remove(productInDb);
                this.db.Update(sale);
            }

            bool productPicturesAreDeleted = await this.pictureService
                .DeletePicturesOfDeletedProductAsync(productId);

            if(productPicturesAreDeleted == false)
            {
                return null;
            }

            this.db.Products.Remove(productInDb);
            int result = await this.db.SaveChangesAsync();

            if (result > 0)
            {
                outcome = productName + "-" + "true";
            }

            return outcome;
        }
    }
}
