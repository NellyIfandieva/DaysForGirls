namespace DaysForGirls.Services
{
    using Services.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICategoryService
    {
        Task<int> CreateAsync(CategoryServiceModel categoryServiceModel);

        Task<IEnumerable<CategoryServiceModel>> DisplayAll();

        Task<bool> DeleteCategoryByIdAsync(int categoryId);

        Task<CategoryServiceModel> GetCategoryByIdAsync(int categoryId);

        Task<bool> EditAsync(CategoryServiceModel model);
    }
}
