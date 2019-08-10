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
        Task<int> Create(SaleServiceModel saleServiceModel);

        IQueryable<SaleServiceModel> DisplayAllAdmin();

        IQueryable<SaleServiceModel> DisplayAll();

        Task<SaleServiceModel> GetSaleByIdAsync(int id);

        Task<bool> AddProductToSale(SaleServiceModel saleToAddTo);
    }
}
