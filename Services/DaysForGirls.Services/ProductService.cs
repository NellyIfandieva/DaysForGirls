using DaysForGirls.Data;
using DaysForGirls.Data.Models;
using DaysForGirls.Services.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DaysForGirls.Services
{
    public class ProductService : IProductService
    {
        private readonly DaysForGirlsDbContext db;
        private readonly IPictureService pictureService;
        private readonly ICustomerReviewService customerReviewService;

        public ProductService(
            DaysForGirlsDbContext db,
            IPictureService pictureService,
            ICustomerReviewService customerReviewService)
        {
            this.db = db;
            this.pictureService = pictureService;
            this.customerReviewService = customerReviewService;
        }

        public async Task<ProductAsShoppingCartItem> GetProductByIdAsync(int productId)
        {
            Product product = await this.db.Products
                .Include(p => p.Quantity)
                .Include(p => p.Pictures)
                .Include(p => p.Sale)
                .SingleOrDefaultAsync(p => p.Id == productId);

            var picture = product.Pictures.ElementAt(0).PictureUrl;

            var productToReturn = new ProductAsShoppingCartItem
            {
                Id = product.Id,
                Name = product.Name,
                Colour = product.Colour,
                Size = product.Size,
                Price = product.Price,
                MainPictureUrl = picture
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

        //public async Task<bool> AddReviewToProductByProductIdAsync(int productId, int reviewId)
        //{
        //    var product = this.db.Products
        //        .SingleOrDefault(p => p.Id == productId);

        //    var review = this.db.CustomerReviews
        //        .SingleOrDefault(r => r.Id == reviewId);

        //    product.Reviews.Add(review);
        //    this.db.Products.Update(product);
        //    int result = await this.db.SaveChangesAsync();

        //    return result > 0;
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

        public async Task<bool> AddProductToShoppingCartAsync(int productId, string shoppingCartId)
        {
            var product = await this.db.Products
                .SingleOrDefaultAsync(p => p.Id == productId);

            product.ShoppingCartId = shoppingCartId;
            product.Quantity.AvailableItems--;

            this.db.Update(product);
            int result = await this.db.SaveChangesAsync();

            bool productQuantityIsUpdated = result > 0;

            return productQuantityIsUpdated;
        }
    }
}
