using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaysForGirls.Data;
using DaysForGirls.Data.Models;
using DaysForGirls.Services.Models;

namespace DaysForGirls.Services
{
    public class ManufacturerService : IManufacturerService
    {
        private readonly DaysForGirlsDbContext db;

        public ManufacturerService(DaysForGirlsDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> Create(ManufacturerServiceModel manufacturerServiceModel)
        {
            Manufacturer manufacturer = new Manufacturer
            {
                Name = manufacturerServiceModel.Name
            };

            this.db.Manufacturers.Add(manufacturer);
            int result = await this.db.SaveChangesAsync();

            return result == 1;
        }

        public IQueryable<ManufacturerServiceModel> DisplayAll()
        {
            var allManufacturers = this.db.Manufacturers
                .Where(mi => mi.IsDeleted == false)
                .Select(m => new ManufacturerServiceModel
                {
                    Id = m.Id,
                    Name = m.Name
                });

            return allManufacturers;
        }
    }
}
