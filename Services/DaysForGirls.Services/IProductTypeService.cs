using DaysForGirls.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaysForGirls.Services
{
    public interface IProductTypeService
    {
        Task<bool> Create(ProductTypeServiceModel prTServiceModel);

        IQueryable<ProductTypeServiceModel> DisplayAll();

        Task<ProductTypeServiceModel> GetProductTypeByIdAsync(int productTypeId);

        Task<bool> EditAsync(ProductTypeServiceModel model);

        Task<bool> DeleteTypeByIdAsync(int productTypeId);
    }
}
