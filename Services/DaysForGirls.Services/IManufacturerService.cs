namespace DaysForGirls.Services
{
    using DaysForGirls.Services.Models;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IManufacturerService
    {
        Task<int> CreateAsync(ManufacturerServiceModel manufacturerServiceModel);

        IQueryable<ManufacturerServiceModel> DisplayAll();

        Task<bool> EditAsync(ManufacturerServiceModel model);

        Task<bool> DeleteManufacturerByIdAsync(int manufacturerId);

        Task<ManufacturerServiceModel> GetManufacturerByIdAsync(int manufacturerId);
    }
}
