using DaysForGirls.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaysForGirls.Services
{
    public interface ISaleService
    {
        Task<string> Create(SaleServiceModel saleServiceModel);

        IQueryable<SaleServiceModel> DisplayAllAdmin();

        IQueryable<SaleServiceModel> DisplayAll();

        Task<SaleServiceModel> GetSaleByIdAsync(string saleId);

        Task<bool> AddProductToSaleAsync(string saleId, int productId);

        Task<bool> DeleteSaleById(string saleId);
    }
}
