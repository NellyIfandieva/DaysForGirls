using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaysForGirls.Services;
using DaysForGirls.Services.Models;
using DaysForGirls.Web.InputModels;
using DaysForGirls.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DaysForGirls.Web.Areas.Administration.Controllers
{
    public class AccessoryController : Controller
    {
        private readonly IAccessoryService accessoryService;
        private readonly ICategoryService categoryService;
        private readonly IManufacturerService manufacturerService;
        private readonly ICloudinaryService cloudinaryService;

        public AccessoryController(
            IAccessoryService accessoryService,
            ICategoryService categoryService,
            IManufacturerService manufacturerService,
            ICloudinaryService cloudinaryService)
        {
            this.accessoryService = accessoryService;
            this.categoryService = categoryService;
            this.manufacturerService = manufacturerService;
            this.cloudinaryService = cloudinaryService;
        }
        public async Task<IActionResult> Create()
        {
            var allCategories = await this.categoryService
               .DisplayAll()
               .ToListAsync();

            this.ViewData["categories"] = allCategories
                .Select(c => new ProductCreateCategoryViewModel
                {
                    Name = c.Name
                })
                .OrderBy(cat => cat.Name)
                .ToList();

            var allManufacturers = await this.manufacturerService
                .DisplayAll()
                .ToListAsync();

            this.ViewData["manufacturers"] = allManufacturers
                .Select(m => new ProductCreateManufacturerViewModel
                {
                    Name = m.Name
                })
                .OrderBy(man => man.Name)
                .ToList();

            await Task.Delay(0);

            return View();
        }

        public async Task<IActionResult> Create(AccessoryCreateInputModel model)
        {

            //TODO Re-do the productType, category and Manuf
            //Add picture to pSM
            //TODO ModelState Validation

            var category = new CategoryServiceModel
            {
                Name = model.Category
            };

            var manufacturer = new ManufacturerServiceModel
            {
                Name = model.Manufacturer
            };

            string mainImageUrl = await this.cloudinaryService
                .UploadPictureForProductAsync(model.MainPicture, model.Name + "_Main");

            AccessoryServiceModel accessoryServiceModel = new AccessoryServiceModel
            {
                Name = model.Name,
                Category = category,
                Description = model.Description,
                MainPicture = new PictureServiceModel
                {
                    PictureUrl = mainImageUrl
                },
                Colour = model.Colour,
                Price = model.Price,
                Manufacturer = manufacturer,
                Quantity = new QuantityServiceModel
                {
                    AvailableItems = model.Quantity
                }
            };

            int accessoryId = await this.accessoryService.Create(accessoryServiceModel);

            //bool isUpdated = await this.pictureService.UpdatePictureInfoAsync(pictureInDbId, productId);
            //when i'm done re-doing the service (and IService)
            //productServiceModel = await this.productService.Create(productServiceModel);

            return Redirect("/Products/AllAccessories");
        }
    }
}