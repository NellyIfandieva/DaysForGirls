using DaysForGirls.Services.Models;
using System.Linq;
using System.Threading.Tasks;

namespace DaysForGirls.Services
{
    public interface IProductService
    {
        Task<int> Create(ProductServiceModel productServiceModel);

        IQueryable<ProductServiceModel> DisplayAll();

        Task<ProductServiceModel> GetProductDetailsById(int Id);

        Task<bool> DeleteProductById(int id);

        IQueryable<ProductServiceModel> GetAllProductsOfCategory(string categoryName);

        IQueryable<ProductServiceModel> GetAllProductsOfTypeAndCategory(
            string productTypeName, string categoryName);

        //IQueryable<ProductServiceModel> AllWeddingProducts();

        //IQueryable<ProductServiceModel> AllWeddingDresses();

        //IQueryable<ProductServiceModel> AllWeddingSuits();

        //IQueryable<ProductServiceModel> AllWeddingAccessories();

        //IQueryable<ProductServiceModel> AllPromProducts();

        //IQueryable<ProductServiceModel> AllPromDresses();

        //IQueryable<ProductServiceModel> AllPromSuits();

        //IQueryable<ProductServiceModel> AllPromAccessories();

        //IQueryable<ProductServiceModel> AllOtherProducts();

        //IQueryable<ProductServiceModel> AllOtherDresses();

        //IQueryable<ProductServiceModel> AllOtherSuits();

        //IQueryable<ProductServiceModel> AllOtherAccessories();

        Task<bool> AddReviewToProductByProductIdAsync(int productId, int reviewId);

        Task<bool> Edit(int productId, ProductServiceModel model);

        //IQueryable<ProductServiceModel> GetAllProductsByCategoryAndType(string productType, string category);

        Task<int> DeletePictureWithUrl(string pictureUrl);

        Task<bool> UploadNewPictureToProduct(int productId, string imageUrl);
    }
}
