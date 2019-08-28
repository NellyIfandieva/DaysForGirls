﻿namespace DaysForGirls.Services
{
    using DaysForGirls.Services.Models;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ISaleService
    {
        Task<string> CreateAsync(SaleServiceModel saleServiceModel);

        IQueryable<SaleServiceModel> DisplayAllAdmin();

        IQueryable<SaleServiceModel> DisplayAll();

        Task<SaleServiceModel> GetSaleByIdAsync(string saleId);

        Task<SaleServiceModel> GetSaleByTitleAsync(string saleName);

        Task<bool> AddProductToSaleAsync(string saleId, int productId);

        Task<bool> DeleteSaleById(string saleId);

        Task<bool> EditAsync(SaleServiceModel model);
    }
}
