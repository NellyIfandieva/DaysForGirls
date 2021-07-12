namespace DaysForGirls.Services
{
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISaleService
    {
        Task<string> CreateAsync(SaleServiceModel saleServiceModel);

        Task<IEnumerable<SaleServiceModel>> DisplayAllAdmin();

        Task<IEnumerable<SaleServiceModel>> DisplayAll();

        Task<SaleServiceModel> GetSaleByIdAsync(string saleId);

        Task<SaleServiceModel> GetSaleByTitleAsync(string saleName);

        Task<int?> AddProductToSaleAsync(string saleId, int productId);

        Task<int?> DeleteSaleById(string saleId);

        Task<int?> EditAsync(SaleServiceModel model);
    }
}
