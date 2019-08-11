
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
        private readonly IPictureService pictureService;

        public ProductService(
            DaysForGirlsDbContext db,
            IPictureService pictureService)
        {
            this.db = db;
            this.pictureService = pictureService;
        }

        public async Task<ProductServiceModel> GetProductByIdAsync(int productId)
        {
            var product = await this.db.Products
                .Include(p => p.Category)
                .Include(p => p.ProductType)
                .Include(p => p.Manufacturer)
                .Include(p => p.Quantity)
                .Include(p => p.Pictures)
                .Include(p => p.Reviews)
                .Include(p => p.Sale)
                .SingleOrDefaultAsync(p => p.Id == productId);

            var productPictures = await this.db.Pictures
                .Where(pic => pic.ProductId == product.Id)
                    .Select(pic => new PictureServiceModel
                    {
                        Id = pic.Id,
                        PictureUrl = pic.PictureUrl
                    }).ToListAsync();

            var productReviews = await this.db.CustomerReviews
                .Where(cR => cR.ProductId == product.Id)
                .Select(pR => new CustomerReviewServiceModel
                {
                    Id = pR.Id,
                    Title = pR.Title,
                    Text = pR.Text,
                    CreatedOn = pR.CreatedOn.ToString("dddd, dd MMMM yyyy"),
                    AuthorUsername = pR.Author.UserName
                }).ToListAsync();

            //var productSales = await this.db.ProductsSales
            //    .Where(s => s.ProductId == product.Id)
            //    .Select(s => new ProductSaleServiceModel
            //    {
            //        Id = s.Id,
            //        SaleId = s.SaleId
            //    }).ToListAsync();

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
                    Name = product.Manufacturer.Name
                },
                Quantity = new QuantityServiceModel
                {
                    AvailableItems = product.Quantity.AvailableItems
                },
                Reviews = productReviews,
                IsDeleted = product.IsDeleted,
                SaleId = product.SaleId
            };

            return productToReturn;
        }

        public IQueryable<ProductDisplayAllServiceModel> DisplayAll()
        {
            var allProducts = this.db.Products
                .Where(p => p.IsDeleted == false)
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
                    },
                    IsInSale = p.IsInSale,
                    SaleId = p.SaleId
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
