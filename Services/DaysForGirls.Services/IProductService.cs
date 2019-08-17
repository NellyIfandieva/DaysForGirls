using DaysForGirls.Services.Models;
using System.Linq;
using System.Threading.Tasks;

namespace DaysForGirls.Services
{
    public interface IProductService
    {
        IQueryable<ProductDisplayAllServiceModel> DisplayAll();

        Task<ProductAsShoppingCartItem> GetProductByIdAsync(int productId);

        IQueryable<DisplayAllOfCategoryProductServiceModel> GetAllProductsOfCategory(string categoryName);

        IQueryable<DisplayAllOfCategoryAndTypeServiceModel> GetAllProductsOfTypeAndCategory(
            string productTypeName, string categoryName);

        Task<bool> AddProductToShoppingCart(int productId, string shoppingCartId);

        //Task<bool> AddReviewToProductByProductIdAsync(int productId, int reviewId);
    }
}
