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
                    .ToList(),
                    ProductsCount = m.Products.Count()
                });

            return allManufacturers;
        }

        public async Task<bool> DeleteManufacturerByIdAsync(int manufacturerId)
        {
            var manufacturerToDelete = await this.db.Manufacturers
                .SingleOrDefaultAsync(m => m.Id == manufacturerId);

            manufacturerToDelete.IsDeleted = true;

            var manufacturerProducts = await this.db.Products
                .Where(p => p.Manufacturer.Id == manufacturerId)
                .ToListAsync();

            foreach(var product in manufacturerProducts)
            {
                if(product.IsDeleted == false)
                {
                    product.IsDeleted = true;
                }
            }

            this.db.UpdateRange(manufacturerProducts);
            this.db.Update(manufacturerToDelete);
            int result = await this.db.SaveChangesAsync();

            bool manufaturerAndProductsAreDeleted = result > 0;

            return manufaturerAndProductsAreDeleted;
        }

        public async Task<ManufacturerServiceModel> GetManufacturerByIdAsync(int manufacturerId)
        {
            var manufacturer = await this.db.Manufacturers
                .Include(m => m.Logo)
                .Include(m => m.Products)
                .SingleOrDefaultAsync(m => m.Id == manufacturerId);

            var logo = await this.db.Logos
                .SingleOrDefaultAsync(l => l.Manufacturer.Id == manufacturer.Logo.Id);

            var manufacturerProducts = await this.db.Products
                .Where(p => p.Manufacturer.Id == manufacturerId
                && p.IsDeleted == false)
                .Select(p => new ProductServiceModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Pictures = p.Pictures
                    .Select(pic => new PictureServiceModel
                    {
                        Id = pic.Id,
                        PictureUrl = pic.PictureUrl
                    }).ToList(),
                    Price = p.Price,
                    SaleId = p.SaleId
                })
                .ToListAsync();

            ManufacturerServiceModel manufacturerToReturn = new ManufacturerServiceModel
            {
                Id = manufacturer.Id,
                Name = manufacturer.Name,
                Description = manufacturer.Description,
                Logo = new LogoServiceModel
                {
                    Id = logo.Id,
                    LogoUrl = logo.LogoUrl
                },
                Products = manufacturerProducts
            };

            return manufacturerToReturn;
        }
    }
}
