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
        Task<bool> Create(SaleServiceModel saleServiceModel);

        IQueryable<SaleServiceModel> DisplayAllAdmin();

        IQueryable<SaleServiceModel> DisplayAll();

        //TODO re-do to return a ProductServiceModel
        SaleServiceModel GetSaleWithDetailsById(int id);

        Task<bool> AddProductToSale(int saleId, int productId);
    }
}
