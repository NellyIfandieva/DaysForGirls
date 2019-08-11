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
        public async Task<IActionResult> Create(ManufacturerCreateInputModel model)
        {
            string imageUrl = await this.cloudinaryService.UploadPictureForProductAsync(
                model.Logo, model.Name + "_" + "Logo");
            ManufacturerServiceModel manufacturerServiceModel = new ManufacturerServiceModel
            {
                Name = model.Name,
                Description = model.Description,
                Logo = new LogoServiceModel
                {
                    LogoUrl = imageUrl
                }
            };

            int newManufacturerId = await this.manufacturerService.Create(manufacturerServiceModel);
            return Redirect("/Administration/Manufacturer/All");
        }

        [HttpGet("/Administration/Manufacturer/All")]
        public async Task<IActionResult> All()
        {
            var allManufacturers = await this.manufacturerService
                .DisplayAll()
                .Select(m => new ManufacturerDisplayAllViewModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    Logo = m.Logo.LogoUrl,
                    IsDeleted = m.IsDeleted,
                    ProductsCount = m.ProductsCount
                })
                .OrderBy(m => m.Name)
                .ToListAsync();

            return View(allManufacturers);
        }

        //TODO implement edit

        //TODO implement delete
    }
}