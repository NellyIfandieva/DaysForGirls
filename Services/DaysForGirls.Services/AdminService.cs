namespace DaysForGirls.Services
{
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Models;
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

        public async Task<int?> CreateAsync(ProductServiceModel productServiceModel)
        {
            var productTypeInDb = db.ProductTypes
                .SingleOrDefault(pT => pT.Name == productServiceModel.ProductType.Name);

            if (productTypeInDb == null)
            {
                return null;
            }

            var categoryInDb = this.db.Categories
                .SingleOrDefault(cat => cat.Name == productServiceModel.Category.Name);

            if (categoryInDb == null)
            {
                return 0;
            }

            var manufacturerInDb = this.db.Manufacturers
                .SingleOrDefault(man => man.Name == productServiceModel.Manufacturer.Name);

            if (manufacturerInDb == null)
            {
                return 0;
            }

            var quantityOfProduct = new Quantity
            {
                AvailableItems = productServiceModel.Quantity.AvailableItems
            };

            this.db.Quantities.Add(quantityOfProduct);

            int result = await this.db.SaveChangesAsync();
            bool quantityIsAdded = result > 0;

            var product = new Product
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
            int createResult = await db.SaveChangesAsync();

            return createResult < 1 ?
                null :
                product.Id;
        }

        public async Task<IEnumerable<AdminProductAllServiceModel>> DisplayAll()
        {
            var allProducts = await this.db
                .Products
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
                }).ToListAsync();

            return allProducts;
        }

        public async Task<ProductServiceModel> GetProductByIdAsync(int productId)
        {
            var productPictures = await this.pictureService
                .GetPicturesOfProductByProductId(productId);

            var productReviews = await this.customerReviewService
                .GetAllCommentsOfProductByProductId(productId);

            var productToReturn = await this.db
                .Products
                .Select(p => new ProductServiceModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    ProductType = new ProductTypeServiceModel
                    {
                        Name = p.ProductType.Name
                    },
                    Category = new CategoryServiceModel
                    {
                        Name = p.Category.Name
                    },
                    Description = p.Description,
                    Pictures = productPictures.ToList(),
                    Colour = p.Colour,
                    Size = p.Size,
                    Price = p.Price,
                    SalePrice = p.SalePrice,
                    Manufacturer = new ManufacturerServiceModel
                    {
                        Id = p.Manufacturer.Id,
                        Name = p.Manufacturer.Name
                    },
                    Quantity = new QuantityServiceModel
                    {
                        AvailableItems = p.Quantity.AvailableItems
                    },
                    Reviews = productReviews.ToList(),
                    IsDeleted = p.IsDeleted,
                    SaleId = p.SaleId,
                    ShoppingCartId = p.ShoppingCartId,
                    OrderId = p.OrderId
                })
                .SingleOrDefaultAsync(p => p.Id == productId);

            return productToReturn;
        }

        public async Task<int?> EditAsync(ProductServiceModel model)
        {
            var productTypeOfProduct = await this.db
                .ProductTypes
                .SingleOrDefaultAsync(pT => pT.Name == model.ProductType.Name);

            var categoryOfProduct = await this.db
                .Categories
                .SingleOrDefaultAsync(c => c.Name == model.Category.Name);

            var manufacturerOfProduct = await this.db
                .Manufacturers
                .SingleOrDefaultAsync(m => m.Name == model.Manufacturer.Name);

            if (productTypeOfProduct == null
                || categoryOfProduct == null
                || manufacturerOfProduct == null)
            {
                return null;
            }

            var productInDb = await this.db
                .Products
                .SingleOrDefaultAsync(p => p.Id == model.Id);

            if(productInDb == null)
            {
                return null;
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
                var picture = new Picture
                {
                    PictureUrl = pic.PictureUrl,
                    ProductId = productInDb.Id,
                    Product = productInDb,
                };

                productInDb.Pictures.Add(picture);
            }

            this.db.Update(productInDb);
            return await db.SaveChangesAsync();
        }

        public async Task<int?> AddProductToSaleAsync(int productId, string saleId)
        {
            var product = await this.db.Products
                .SingleOrDefaultAsync(p => p.Id == productId);

            var sale = await this.db.Sales
                .SingleOrDefaultAsync(s => s.Id == saleId);

            if (product == null || sale == null)
            {
                return null;
            }

            product.SaleId = sale.Id;
            product.IsInSale = true;

            this.db.Products.Update(product);
            return await this.db.SaveChangesAsync();
        }

        private async Task<IEnumerable<Product>> GetAllProductsByIds(List<int> productIds)
        {
            var allSearchedProducts = await this.db.Products
                .Where(p => productIds.Contains(p.Id) && 
                       p.IsDeleted == false)
                .ToListAsync();

            return allSearchedProducts;
        }

        public async Task<int?> SetProductsCartIdToNullAsync(List<int> productIds)
        {
            var products = await GetAllProductsByIds(productIds);

            if(products.Any() == false)
            {
                return null;
            }

            foreach (var product in products)
            {
                product.Quantity.AvailableItems++;
                product.ShoppingCartId = null;
            }

            this.db.UpdateRange(products);
            return await this.db.SaveChangesAsync();
        }

        public async Task<int?> SetOrderIdToProductsAsync(
            List<int> productIds, 
            string orderId)
        {
            if(productIds.Count < 1 || 
                orderId == null)
            {
                return null;
            }

            var productsToAddToOrder = await this.db
                    .Products
                    .Where(p => productIds.Contains(p.Id))
                    .ToListAsync();

            if (productsToAddToOrder.Count < 1)
            {
                return null;
            }

            foreach (var product in productsToAddToOrder)
            {
                product.OrderId = orderId;
                product.ShoppingCartId = null;
            }

            this.db.UpdateRange(productsToAddToOrder);
            return await this.db.SaveChangesAsync();
        }

        public async Task<string> EraseFromDb(int productId)
        {
            var productInDb = await this.db
                .Products
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

            var productPicturesAreDeleted = await this.pictureService
                .DeletePicturesOfDeletedProductAsync(productId);

            if(productPicturesAreDeleted == null)
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
