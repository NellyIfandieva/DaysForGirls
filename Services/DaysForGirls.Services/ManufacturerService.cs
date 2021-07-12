namespace DaysForGirls.Services
{
    using DaysForGirls.Data;
    using DaysForGirls.Data.Models;
    using DaysForGirls.Services.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ManufacturerService : IManufacturerService
    {
        private readonly DaysForGirlsDbContext db;

        public ManufacturerService(DaysForGirlsDbContext db)
        {
            this.db = db;
        }

        public async Task<int> CreateAsync(ManufacturerServiceModel manufacturerServiceModel)
        {
            var manufacturer = new Manufacturer
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

        public async Task<ManufacturerServiceModel> GetManufacturerByIdAsync(int manufacturerId)
        {
            if(manufacturerId <= 0)
            {
                return null;
            }

            var manufacturer = await this.db.Manufacturers
                .Include(m => m.Logo)
                .Include(m => m.Products)
                .SingleOrDefaultAsync(m => m.Id == manufacturerId);

            if (manufacturer == null)
            {
                return null;
            }

            var logo = await this.db.Logos
                .SingleOrDefaultAsync(l => l.Manufacturer.Id == manufacturer.Id);

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
                    Quantity = new QuantityServiceModel
                    {
                        AvailableItems = p.Quantity.AvailableItems
                    },
                    SaleId = p.SaleId,
                    ShoppingCartId = p.ShoppingCartId,
                    OrderId = p.OrderId
                })
                .ToListAsync();

            var manufacturerToReturn = new ManufacturerServiceModel
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

        public async Task<IEnumerable<ManufacturerServiceModel>> DisplayAll()
        {
            var allManufacturers = await this.db.Manufacturers
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
                })
                .ToListAsync();

            return allManufacturers;
        }

        public async Task<bool> EditAsync(ManufacturerServiceModel model)
        {
            var manufacturerInDb = await this.db.Manufacturers
                .Include(m => m.Logo)
                .SingleOrDefaultAsync(m => m.Id == model.Id);

            if (manufacturerInDb == null)
            {
                return false;
            }

            manufacturerInDb.Name = model.Name;
            manufacturerInDb.Description = model.Description;
            manufacturerInDb.Logo.LogoUrl = model.Logo.LogoUrl;

            this.db.Update(manufacturerInDb);
            int result = await this.db.SaveChangesAsync();

            bool manufacturerIsEdited = result > 0;

            return manufacturerIsEdited;
        }

        public async Task<bool> DeleteManufacturerByIdAsync(int manufacturerId)
        {
            var manufacturerToDelete = await this.db.Manufacturers
                .SingleOrDefaultAsync(m => m.Id == manufacturerId);

            if (manufacturerToDelete == null)
            {
                return false;
            }

            var manufacturerProducts = this.db.Products
                .Where(p => p.Manufacturer.Id == manufacturerId);

            if (manufacturerProducts.Any())
            {
                manufacturerToDelete.IsDeleted = true;
                this.db.Update(manufacturerToDelete);
            }
            else
            {
                var logoToDelete = await this.db.Logos
                    .SingleOrDefaultAsync(l => l.ManufacturerId == manufacturerToDelete.Id);

                if (logoToDelete == null)
                {
                    throw new ArgumentNullException(nameof(logoToDelete));
                }

                this.db.Logos.Remove(logoToDelete);
                this.db.Manufacturers.Remove(manufacturerToDelete);
            }

            int result = await this.db.SaveChangesAsync();

            bool manufacturerIsDeleted = result > 0;

            return manufacturerIsDeleted;
        }
    }
}
