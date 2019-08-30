namespace DaysForGirls.Services
{
    using DaysForGirls.Services.Models;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ICategoryService
    {
        Task<int> CreateAsync(CategoryServiceModel categoryServiceModel);

        IQueryable<CategoryServiceModel> DisplayAll();

        Task<bool> DeleteCategoryByIdAsync(int categoryId);

        Task<CategoryServiceModel> GetCategoryByIdAsync(int categoryId);

        Task<bool> EditAsync(CategoryServiceModel model);
    }
}
