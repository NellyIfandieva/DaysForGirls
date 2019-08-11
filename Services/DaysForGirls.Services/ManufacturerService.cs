using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaysForGirls.Data;
using DaysForGirls.Data.Models;
using DaysForGirls.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace DaysForGirls.Services
{
    public class ManufacturerService : IManufacturerService
    {
        private readonly DaysForGirlsDbContext db;

        public ManufacturerService(DaysForGirlsDbContext db)
        {
            this.db = db;
        }

        public async Task<int> Create(ManufacturerServiceModel manufacturerServiceModel)
        {
            Manufacturer manufacturer = new Manufacturer
            {
                Name = manufacturerServiceModel.Name,
                Description = manufacturerServiceModel.Description,
                Logo = new Logo
                {
                    LogoUrl = manufacturerServiceModel.Logo.LogoUrl
                }
            };

            this.db.Manufacturers.Add(manufacturer);
            int result = await this.db.SaveChangesAsync();

            int manufacturerId = manufacturer.Id;

            return manufacturerId;
        }

        public IQueryable<ManufacturerServiceModel> DisplayAll()
        {
            var allManufacturers = this.db.Manufacturers
                .Where(mi => mi.IsDeleted == false)
                .Include(m => m.Products)
                .Select(m => new ManufacturerServiceModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    Logo = new LogoServiceModel
                    {
                        LogoUrl = m.Logo.LogoUrl
                    },
                    IsDeleted = m.IsDeleted,
                    Products = m.Products
                    .Where(p => p.ManufacturerId == m.Id)
                    .Select(p => new ProductServiceModel
                    {
                        Id = p.Id
                    })
                    .ToList()
                });

            return allManufacturers;
        }
    }
}
