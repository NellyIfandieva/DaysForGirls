namespace DaysForGirls.Services
{
    using DaysForGirls.Services.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IProductService
    {
        Task<IEnumerable<ProductDisplayAllServiceModel>> DisplayAll();

        Task<ProductAsShoppingCartItem> GetProductByIdAsync(int productId);

        Task<IEnumerable<DisplayAllOfCategoryProductServiceModel>> GetAllProductsOfCategory(string categoryName);

        Task<IEnumerable<DisplayAllOfCategoryAndTypeServiceModel>> GetAllProductsOfTypeAndCategory(
            string productTypeName, string categoryName);

        Task<bool> AddProductToShoppingCartAsync(int productId, string shoppingCartId);

        Task<bool> RemoveProductFromShoppingCartAsync(int productId);

        Task<IEnumerable<ProductServiceModel>> GetAllSearchResultsByCriteria(string criteria);

        Task<decimal> CalculateProductPriceAsync(int productId);
    }
}
