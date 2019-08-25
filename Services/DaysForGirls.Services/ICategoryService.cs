using DaysForGirls.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaysForGirls.Services
{
    public interface ICategoryService
    {
        Task<int> CreateAsync(CategoryServiceModel categoryServiceModel);

        IQueryable<CategoryServiceModel> DisplayAll();

        Task<bool> DeleteCategoryByIdAsync(int categoryId);

        Task<CategoryServiceModel> GetCategoryByIdAsync(int categoryId);

        Task<bool> EditAsync(CategoryServiceModel model);
    }
}
