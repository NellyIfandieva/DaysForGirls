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

        public ManufacturerController(IManufacturerService manufacturerService)
        {
            this.manufacturerService = manufacturerService;
        }

        [HttpGet("/Administration/Manufacturer/Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("/Administration/Manufacturer/Create")]
        public async Task<IActionResult> Create(ManufacturerCreateInputModel model)
        {
            ManufacturerServiceModel manufacturerServiceModel = new ManufacturerServiceModel
            {
                Name = model.Name
            };

            bool isCreated = await this.manufacturerService.Create(manufacturerServiceModel);
            return Redirect("/Administration/Manufacturer/All");
        }

        [HttpGet("/Administration/Manufacturer/All")]
        public async Task<IActionResult> All()
        {
            var allManufacturers = await this.manufacturerService
                .DisplayAll()
                .OrderBy(m => m.Name)
                .ToListAsync();

            var allManufacturersToDisplay = new HashSet<ManufacturerDisplayAllViewModel>();

            foreach(var manufacturer in allManufacturers)
            {
                ManufacturerDisplayAllViewModel vm = new ManufacturerDisplayAllViewModel
                {
                    Name = manufacturer.Name
                };

                allManufacturersToDisplay.Add(vm);
            }

            return View(allManufacturersToDisplay);
        }

        //TODO implement 
    }
}