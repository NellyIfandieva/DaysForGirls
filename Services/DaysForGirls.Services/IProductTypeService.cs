namespace DaysForGirls.Services
{
    using DaysForGirls.Services.Models;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IProductTypeService
    {
        Task<bool> CreateAsync(ProductTypeServiceModel prTServiceModel);

        IQueryable<ProductTypeServiceModel> DisplayAll();

        Task<ProductTypeServiceModel> GetProductTypeByIdAsync(int productTypeId);

        Task<bool> EditAsync(ProductTypeServiceModel model);

        Task<bool> DeleteTypeByIdAsync(int productTypeId);
    }
}
