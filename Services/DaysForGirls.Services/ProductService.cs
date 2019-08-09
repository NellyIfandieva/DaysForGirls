
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DaysForGirls.Data;
using DaysForGirls.Data.Models;
using DaysForGirls.Services.Models;

namespace DaysForGirls.Services
{
    public class ProductService : IProductService
    {
        private readonly DaysForGirlsDbContext db;
        private readonly IAdminService adminService;
        private readonly IPictureService pictureService;

        public ProductService(
            DaysForGirlsDbContext db,
            IPictureService pictureService)
        {
            this.db = db;
            this.pictureService = pictureService;
        }

        public async Task<ProductServiceModel> GetProductDetailsById(int productId)
        {
            var productInDb = this.db.Products
                .SingleOrDefault(p => p.Id == productId);

            var productPictures = productInDb.Pictures;

            var productReviews = productInDb.Reviews;

            //var productPictures = this.db.Pictures
            //    .Where(p => p.ProductId == productInDb.Id)
            //    .Select(p => new PictureServiceModel
            //    {
            //        Id = p.Id,
            //        PictureUrl = p.PictureUrl,
            //        ProductId = p.ProductId
            //    })
            //    .ToList();

            //var productCustomerReviews = this.db.CustomerReviews
            //    .Where(c => c.ProductId == productInDb.Id)
            //    .Select(c => new CustomerReviewServiceModel
            //    {
            //        Id = c.Id,
            //        Title = c.Title,
            //        Text = c.Text,
            //        CreatedOn = c.CreatedOn.ToString(),
            //        Author = new DaysForGirlsUserServiceModel
            //        {
            //            UserName = c.Author.UserName
            //        }
            //    })
            //    .ToList();

            var productTypeOfProduct = this.db.ProductTypes
                .SingleOrDefault(pT => pT.Id == productInDb.ProductTypeId);

            var categoryOfProduct = this.db.Categories
                .SingleOrDefault(c => c.Id == productInDb.CategoryId);

            var manufacturerOfProduct = this.db.Manufacturers
                .SingleOrDefault(m => m.Id == productInDb.ManufacturerId);

            var quantityOfProduct = this.db.Quantities
                .SingleOrDefault(q => q.Id == productInDb.QuantityId);

            productInDb.Category = categoryOfProduct;
            productInDb.ProductType = productTypeOfProduct;
            productInDb.Manufacturer = manufacturerOfProduct;
            productInDb.Quantity = quantityOfProduct;

            ProductServiceModel productToReturn = new ProductServiceModel
            {
                Id = productInDb.Id,
                Name = productInDb.Name,
                ProductType = new ProductTypeServiceModel
                {
                    Name = productInDb.ProductType.Name
                },
                Category = new CategoryServiceModel
                {
                    Name = productInDb.Category.Name
                },
                Description = productInDb.Description,
                Colour = productInDb.Colour,
                Size = productInDb.Size,
                Price = productInDb.Price,
                Manufacturer = new ManufacturerServiceModel
                {
                    Name = productInDb.Manufacturer.Name
                },
                Quantity = new QuantityServiceModel
                {
                    AvailableItems = productInDb.Quantity.AvailableItems
                },
                Pictures = productPictures
                    .Select(pic => new PictureServiceModel
                    {

                    }).ToList(),
                Reviews = productReviews
                    .Select(r => new CustomerReviewServiceModel
                    {
                        Id = r.Id,
                        Title = r.Title,
                        Text = r.Text,
                        CreatedOn = r.CreatedOn.ToString("dddd dd MMMM yyyy"),
                        AuthorUsername = r.Author.UserName
                    }).ToList()
            };

            await Task.Delay(0);
            return productToReturn;
        }

        public IQueryable<ProductDisplayAllServiceModel> DisplayAll()
        {
            var allProducts = this.db.Products
                .Select(p => new ProductDisplayAllServiceModel
                 {
                     Id = p.Id,
                     Name = p.Name,
                     Price = p.Price,
                     Picture = new PictureServiceModel
                     {
                        PictureUrl = p.Pictures.ElementAt(0).PictureUrl
                     }
                 });

            return allProducts;
        }

        public IQueryable<DisplayAllOfCategoryProductServiceModel> GetAllProductsOfCategory(string categoryName)
        {
            var allProductsOfCategory = this.db.Products
                .Where(p => p.Category.Name == categoryName
                && p.IsDeleted == false)
                .Select(p => new DisplayAllOfCategoryProductServiceModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Picture = new PictureServiceModel
                    {
                        PictureUrl = p.Pictures.ElementAt(0).PictureUrl
                    }
                });

            return allProductsOfCategory;
        }

        public IQueryable<DisplayAllOfCategoryAndTypeServiceModel> GetAllProductsOfTypeAndCategory(string productType, string category)
        {
            var allProductsOfCategoryAndType = this.db.Products
                .Where(p => p.Category.Name == category
                && p.ProductType.Name == productType
                && p.IsDeleted == false)
                .Select(p => new DisplayAllOfCategoryAndTypeServiceModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Picture = new PictureServiceModel
                    {
                        PictureUrl = p.Pictures.ElementAt(0).PictureUrl
                    },
                    Price = p.Price
                });

            return allProductsOfCategoryAndType;
        }

        public async Task<bool> AddReviewToProductByProductIdAsync(int productId, int reviewId)
        {
            var product = this.db.Products
                .SingleOrDefault(p => p.Id == productId);

            var review = this.db.CustomerReviews
                .SingleOrDefault(r => r.Id == reviewId);

            product.Reviews.Add(review);
            this.db.Products.Update(product);
            int result = await this.db.SaveChangesAsync();

            return result > 0;
        }

        //public async Task<bool> Edit(int productId, ProductServiceModel model)
        //{
        //    ProductType productTypeOfProduct = await this.db.ProductTypes
        //        .SingleOrDefaultAsync(pT => pT.Name == model.ProductType.Name);

        //    Category categoryOfProduct = await this.db.Categories
        //        .SingleOrDefaultAsync(c => c.Name == model.Category.Name);

        //    Manufacturer manufacturerOfProduct = await this.db.Manufacturers
        //        .SingleOrDefaultAsync(m => m.Name == model.Manufacturer.Name);

        //    //if (productTypeFromDb == null)
        //    //{
        //    //    throw new ArgumentNullException(nameof(productTypeFromDb));
        //    //}

        //    Product productInDb = await this.db.Products
        //        .SingleOrDefaultAsync(p => p.Id == productId);

        //    //if (productFromDb == null)
        //    //{
        //    //    throw new ArgumentNullException(nameof(productFromDb));
        //    //}

        //    productInDb.Name = model.Name;
        //    productInDb.ProductType = productTypeOfProduct;
        //    productInDb.Category = categoryOfProduct;
        //    productInDb.Description = model.Description;
        //    productInDb.Colour = model.Colour;
        //    productInDb.Size = model.Size;
        //    productInDb.Price = model.Price;
        //    productInDb.Manufacturer = manufacturerOfProduct;
        //    productInDb.QuantityId = model.Quantity.Id;

        //    this.db.Products.Update(productInDb);
        //    int result = await db.SaveChangesAsync();

        //    bool editsApplied = result > 0;

        //    return editsApplied;
        //}

        public async Task<bool> DeletePictureWithUrl(string pictureUrl)
        {
            Picture pictureToDelete = this.db.Pictures
                .SingleOrDefault(pic => pic.PictureUrl == pictureUrl);

            var product = await this.db.Products
                .SingleOrDefaultAsync(p => p.Id == pictureToDelete.ProductId);

            product.Pictures.Remove(pictureToDelete);

            pictureToDelete.IsDeleted = true;

            this.db.UpdateRange(pictureToDelete, product);

            int result = await this.db.SaveChangesAsync();
            bool pictureIsDeleted = result > 0;

            return pictureIsDeleted;
        }

        //public async Task<bool> UploadNewPictureToProduct(int productId, string imageUrl)
        //{
        //var productInDb = await this.db.Products.
        //    SingleOrDefaultAsync(p => p.Id == productId);

        //Picture newPicture = new Picture
        //{
        //    PictureUrl = imageUrl
        //};

        //productInDb.Pictures.Add(newPicture);
        //    this.db.Update(productInDb);
        //    int result = await this.db.SaveChangesAsync();

        //bool pictureIsAdded = result > 0;

        //    return pictureIsAdded;
        //}

        //public async Task<bool> AddProductToSale(int productId, int saleId)
        //{
        //    var productSale = this.db.ProductsSales
        //        .SingleOrDefault(pS => pS.ProductId == productId
        //        && pS.SaleId == saleId);

        //    var product = this.db.Products
        //        .SingleOrDefault(p => p.Id == productId);

        //    product.Sales.Add(productSale);
        //    product.IsInSale = true;

        //    this.db.Products.Update(product);
        //    int result = await this.db.SaveChangesAsync();
        //    bool productIsAddedToSale = result > 0;

        //    return productIsAddedToSale;
        //}

        public async Task<bool> UpdateProductQuantity(int productId)
        {
            var product = this.db.Products
                .SingleOrDefault(p => p.Id == productId);

            if(product != null)
            {
                product.Quantity.AvailableItems--;
            }

            this.db.Update(product);
            int result = await this.db.SaveChangesAsync();

            bool productQuantityIsUpdated = result > 0;

            return productQuantityIsUpdated;
        }
    }
}
