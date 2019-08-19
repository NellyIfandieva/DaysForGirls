using DaysForGirls.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaysForGirls.Services
{
    public interface IManufacturerService
    {
        Task<int> Create(ManufacturerServiceModel manufacturerServiceModel);

        IQueryable<ManufacturerServiceModel> DisplayAll();

        Task<bool> DeleteManufacturerByIdAsync(int manufacturerId);

        Task<ManufacturerServiceModel> GetManufacturerByIdAsync(int manufacturerId);
    }
}
