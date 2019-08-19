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
        Task<int> Create(CategoryServiceModel categoryServiceModel);

        IQueryable<CategoryServiceModel> DisplayAll();

        Task<bool> DeleteCategoryByIdAsync(int categoryId);
    }
}
