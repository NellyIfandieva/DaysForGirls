namespace DaysForGirls.Services
{
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProductTypeService
    {
        Task<int?> CreateAsync(ProductTypeServiceModel prTServiceModel);

        Task<IEnumerable<ProductTypeServiceModel>> DisplayAll();

        Task<ProductTypeServiceModel> GetProductTypeByIdAsync(int productTypeId);

        Task<int?> EditAsync(ProductTypeServiceModel model);

        Task<int?> DeleteTypeByIdAsync(int productTypeId);
    }
}
