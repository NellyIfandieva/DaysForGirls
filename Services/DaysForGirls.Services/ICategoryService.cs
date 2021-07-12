namespace DaysForGirls.Services
{
    using Services.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICategoryService
    {
        Task<int?> CreateAsync(CategoryServiceModel categoryServiceModel);

        Task<IEnumerable<CategoryServiceModel>> DisplayAll();

        Task<int?> DeleteCategoryByIdAsync(int categoryId);

        Task<CategoryServiceModel> GetCategoryByIdAsync(int categoryId);

        Task<int?> EditAsync(CategoryServiceModel model);
    }
}
