namespace DaysForGirls.Web.Areas.Administration.Controllers
{
    using InputModels;
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using Services.Models;
    using System.Linq;
    using System.Threading.Tasks;
    using ViewModels;

    public class ManufacturerController : AdminController
    {
        private readonly IManufacturerService manufacturerService;
        private readonly ICloudinaryService cloudinaryService;

        public ManufacturerController(
            IManufacturerService manufacturerService,
            ICloudinaryService cloudinaryService)
        {
            this.manufacturerService = manufacturerService;
            this.cloudinaryService = cloudinaryService;
        }

        [HttpGet("/Administration/Manufacturer/Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("/Administration/Manufacturer/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ManufacturerCreateInputModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            string imageUrl = await this.cloudinaryService
                .UploadLogoForManufacturerAsync(
                model.Logo, model.Name + "_" + "Logo");

            var manufacturerServiceModel = new ManufacturerServiceModel
            {
                Name = model.Name,
                Description = model.Description,
                Logo = new LogoServiceModel
                {
                    LogoUrl = imageUrl
                }
            };

            var createResult = await this.manufacturerService
                .CreateAsync(manufacturerServiceModel);

            if(createResult == null)
            {
                return Redirect("/Home/Error");
            }

            return Redirect("/Administration/Manufacturer/All");
        }

        [HttpGet("/Administration/Manufacturer/All")]
        public async Task<IActionResult> All()
        {
            var allManufacturers = await this.manufacturerService
                .DisplayAll();

            var viewModels = allManufacturers
                .Select(m => new ManufacturerDisplayAllViewModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    Logo = m.Logo.LogoUrl,
                    IsDeleted = m.IsDeleted,
                    ProductsCount = m.ProductsCount
                })
                .OrderBy(m => m.Name);

            return View(viewModels);
        }

        [HttpGet("/Administration/Manufacturer/Edit/{manufacturerId}")]
        public async Task<IActionResult> Edit(int manufacturerId)
        {
            if (manufacturerId <= 0)
            {
                return Redirect("/Home/Error");
            }

            var manufacturerFromDb = await this.manufacturerService
                .GetManufacturerByIdAsync(manufacturerId);

            if(manufacturerFromDb == null)
            {
                return Redirect("Home/Error");
            }

            var manufacturerToEdit = new ManufacturerEditInputModel
            {
                Id = manufacturerFromDb.Id,
                Name = manufacturerFromDb.Name,
                Description = manufacturerFromDb.Description
            };

            return View(manufacturerToEdit);
        }

        [HttpPost("/Administration/Manufacturer/Edit/{manufacturerId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int manufacturerId, ManufacturerEditInputModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            var imageUrl = await this.cloudinaryService
                .UploadPictureForProductAsync(
                model.Logo, model.Name + "_" + "Logo");

            var manufacturerToEdit = new ManufacturerServiceModel
            {
                Id = manufacturerId,
                Name = model.Name,
                Description = model.Description,
                Logo = new LogoServiceModel
                {
                    LogoUrl = imageUrl
                }
            };

            var editResult = await this.manufacturerService
                .EditAsync(manufacturerToEdit);

            if(editResult == null)
            {
                return Redirect("/Home/Error");
            }

            return Redirect("/Administration/Manufacturer/All");
        }

        [HttpGet("/Administration/Manufacturer/Delete/{manufacturerId}")]
        public async Task<IActionResult> Delete(int manufacturerId)
        {
            if (manufacturerId <= 0)
            {
                return Redirect("/Home/Error");
            }

            var deleteResult = await this.manufacturerService
                .DeleteManufacturerByIdAsync(manufacturerId);

            if (deleteResult == null)
            {
                return Redirect("/Home/Error");
            }

            return Redirect("/Administration/Manufacturer/All");
        }
    }
}