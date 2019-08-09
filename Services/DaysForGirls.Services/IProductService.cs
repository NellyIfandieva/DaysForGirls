using DaysForGirls.Services.Models;
using System.Linq;
using System.Threading.Tasks;

namespace DaysForGirls.Services
{
    public interface IProductService
    {
        IQueryable<ProductDisplayAllServiceModel> DisplayAll();

        Task<ProductServiceModel> GetProductDetailsById(int Id);

        IQueryable<DisplayAllOfCategoryProductServiceModel> GetAllProductsOfCategory(string categoryName);

        IQueryable<DisplayAllOfCategoryAndTypeServiceModel> GetAllProductsOfTypeAndCategory(
            string productTypeName, string categoryName);

        Task<bool> UpdateProductQuantity(int productId);

        Task<bool> AddReviewToProductByProductIdAsync(int productId, int reviewId);
    }
}
