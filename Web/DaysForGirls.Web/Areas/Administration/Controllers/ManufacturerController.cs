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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ManufacturerCreateInputModel model)
        {
            if(ModelState.IsValid == false)
            {
                return View(model);
            }

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

            int newManufacturerId = await this.manufacturerService.CreateAsync(manufacturerServiceModel);
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
                    IsDeleted = m.IsDeleted
                })
                .OrderBy(m => m.Name)
                .ToListAsync();

            if(allManufacturers == null)
            {
                return NotFound();
            }

            return View(allManufacturers);
        }

        [HttpGet("/Administration/Manufacturer/Edit/{manufacturerId}")]
        public async Task<IActionResult> Edit(int manufacturerId)
        {
            if(manufacturerId <= 0)
            {
                return BadRequest();
            }

            var manufacturerFromDb = await this.manufacturerService
                .GetManufacturerByIdAsync(manufacturerId);

            if(manufacturerFromDb == null)
            {
                return NotFound();
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
        public async Task<IActionResult> Edit(int manufacturerId, ManufacturerEditInputModel model)
        {
            if(ModelState.IsValid == false)
            {
                return View(model);
            }

            string imageUrl = await this.cloudinaryService.UploadPictureForProductAsync(
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

            bool manufacturerIsEdited = await this.manufacturerService
                .EditAsync(manufacturerToEdit);

            return Redirect("/Administration/Manufacturer/All");
        }

        [HttpGet("/Administration/Manufacturer/Delete/{manufacturerId}")]
        public async Task<IActionResult> Delete(int manufacturerId)
        {
            if(manufacturerId <= 0)
            {
                return BadRequest();
            }

            bool manufacturerIsDeleted = await this.manufacturerService
                .DeleteManufacturerByIdAsync(manufacturerId);

            return Redirect("/Administration/Manufacturer/All");
        }
    }
}