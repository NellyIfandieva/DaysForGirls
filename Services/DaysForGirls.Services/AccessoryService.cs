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
    public class AccessoryService : IAccessoryService
    {
        //private readonly DaysForGirlsDbContext db;
        //private readonly IQuantityService quantityService;

        //public AccessoryService(
        //    DaysForGirlsDbContext db,
        //    IQuantityService quantityService)
        //{
        //    this.db = db;
        //    this.quantityService = quantityService;
        //}

        //public async Task<int> Create(AccessoryServiceModel accessoryServiceModel)
        //{
        //    Category categoryInDb = this.db.Categories
        //        .SingleOrDefault(cat => cat.Name == accessoryServiceModel.Category.Name);

        //    Manufacturer manufacturerInDb = this.db.Manufacturers
        //        .SingleOrDefault(man => man.Name == accessoryServiceModel.Manufacturer.Name);

        //    QuantityServiceModel accessoryQuantity = new QuantityServiceModel
        //    {
        //        AvailableItems = accessoryServiceModel.Quantity.AvailableItems
        //    };

        //    accessoryQuantity = await this.quantityService.Create(accessoryQuantity);

        //    Accessory accessory = new Accessory
        //    {
        //        Name = accessoryServiceModel.Name,
        //        Category = categoryInDb,
        //        MainPicture = accessoryServiceModel.MainPicture.PictureUrl,
        //        Description = accessoryServiceModel.Description,
        //        Colour = accessoryServiceModel.Colour,
        //        Price = accessoryServiceModel.Price,
        //        Manufacturer = manufacturerInDb,
        //        QuantityId = accessoryQuantity.Id
        //    };

        //    this.db.Accessories.Add(accessory);
        //    int result = await db.SaveChangesAsync();

        //    //productServiceModel.Id = product.Id;

        //    return accessory.Id;
        //}

        //public IQueryable<AccessoryServiceModel> AllAccessories()
        //{
        //    var allAccessories = this.db.Accessories
        //        .Select(a => new AccessoryServiceModel
        //        {
        //            Id = a.Id,
        //            Name = a.Name,
        //            Price = a.Price,
        //            Category = new CategoryServiceModel
        //            {
        //                Name = a.Category.Name
        //            },
        //            MainPicture = new PictureServiceModel
        //            {
        //                PictureUrl = a.MainPicture
        //            },
        //            Quantity = new QuantityServiceModel
        //            {
        //                AvailableItems = a.Quantity.AvailableItems
        //            }
        //        });

        //    return allAccessories;
        //}

        //public IQueryable<AccessoryServiceModel> AllWeddingAccessories()
        //{
        //    var allAccessories = this.db.Accessories
        //        .Where(a => a.Category.Name == "Wedding")
        //        .Select(accessory => new AccessoryServiceModel
        //        {
        //            Id = accessory.Id,
        //            Name = accessory.Name,
        //            Category = new CategoryServiceModel
        //            {
        //                Name = accessory.Category.Name
        //            },
        //            Description = accessory.Description,
        //            MainPicture = new PictureServiceModel
        //            {
        //                PictureUrl = accessory.MainPicture
        //            },
        //            Colour = accessory.Colour,
        //            Price = accessory.Price,
        //            Manufacturer = new ManufacturerServiceModel
        //            {
        //                Name = accessory.Manufacturer.Name
        //            },
        //            Quantity = new QuantityServiceModel
        //            {
        //                AvailableItems = accessory.Quantity.AvailableItems
        //            }
        //        });

        //    return allAccessories;
        //}
    }
}
