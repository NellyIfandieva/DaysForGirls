namespace DaysForGirls.Services
{
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IManufacturerService
    {
        Task<int?> CreateAsync(ManufacturerServiceModel manufacturerServiceModel);

        Task<IEnumerable<ManufacturerServiceModel>> DisplayAll();

        Task<int?> EditAsync(ManufacturerServiceModel model);

        Task<int?> DeleteManufacturerByIdAsync(int manufacturerId);

        Task<ManufacturerServiceModel> GetManufacturerByIdAsync(int manufacturerId);
    }
}
