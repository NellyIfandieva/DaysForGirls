namespace DaysForGirls.Services
{
    using DaysForGirls.Services.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAdminService
    {
        Task<int?> CreateAsync(ProductServiceModel productServiceModel);

        Task<IEnumerable<AdminProductAllServiceModel>> DisplayAll();

        Task<ProductServiceModel> GetProductByIdAsync(int productId);

        Task<int?> SetProductsCartIdToNullAsync(List<int> productIds);

        Task<int?> EditAsync(ProductServiceModel model);

        Task<int?> AddProductToSaleAsync(int productId, string saleId);

        Task<int?> SetOrderIdToProductsAsync(List<int> productIds, string orderId);

        Task<string> EraseFromDb(int productId);
    }
}
