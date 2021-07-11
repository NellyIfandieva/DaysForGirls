namespace DaysForGirls.Services
{
    using DaysForGirls.Services.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAdminService
    {
        Task<int> CreateAsync(ProductServiceModel productServiceModel);

        Task<IEnumerable<AdminProductAllServiceModel>> DisplayAll();

        Task<ProductServiceModel> GetProductByIdAsync(int productId);

        Task<bool> SetProductsCartIdToNullAsync(List<int> productIds);

        Task<bool> EditAsync(ProductServiceModel model);

        Task<bool> AddProductToSaleAsync(int productId, string saleId);

        Task<bool> SetOrderIdToProductsAsync(List<int> productIds, string orderId);

        Task<string> EraseFromDb(int productId);
    }
}
