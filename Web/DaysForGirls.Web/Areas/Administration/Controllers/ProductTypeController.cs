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
    public class ProductTypeController : AdminController
    {
        private readonly IProductTypeService productTypeService;

        public ProductTypeController(IProductTypeService productTypeService)
        {
            this.productTypeService = productTypeService;
        }

        [HttpGet("/Administration/ProductType/Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("/Administration/ProductType/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductTypeCreateInputModel model)
        {
            if(ModelState.IsValid == false)
            {
                return View(model);
            }

            ProductTypeServiceModel pTServiceModel = new ProductTypeServiceModel
            {
                Name = model.Name
            };

            bool isCreated = await this.productTypeService.Create(pTServiceModel);

            if(isCreated == false)
            {
                //TODO what do I return in this case? 
            }

            return Redirect("/Administration/ProductType/All");
        }

        public async Task<IActionResult> All()
        {
            var allProductTypes = await this.productTypeService
                .DisplayAll()
                .Select(pT => new ProductTypeDisplayAllViewModel
                {
                    Id = pT.Id,
                    Name = pT.Name
                })
                .OrderBy(pt => pt.Name)
                .ToListAsync();

            if(allProductTypes == null)
            {
                return NotFound();
            }

            return View(allProductTypes);
        }

        [HttpGet("/Administration/ProductType/Edit/{productTypeId}")]
        public async Task<IActionResult> Edit(int productTypeId)
        {
            if(productTypeId <= 0)
            {
                return BadRequest();
            }

            var productTypeFromDb = await this.productTypeService
                .GetProductTypeByIdAsync(productTypeId);

            var productTypeToEdit = new ProductTypeEditInputModel
            {
                Id = productTypeId,
                Name = productTypeFromDb.Name,
                IsDeleted = productTypeFromDb.IsDeleted
            };

            return View(productTypeToEdit);
        }

        [HttpPost("/Administration/ProductType/Edit/{productTypeId}")]
        public async Task<IActionResult> Edit(int productTypeId, ProductTypeEditInputModel model)
        {
            var productTypeWithEdits = new ProductTypeServiceModel
            {
                Id = productTypeId,
                Name = model.Name
            };

            bool productTypeIsEdited = await this.productTypeService
                .EditAsync(productTypeWithEdits);

            return Redirect("/Administration/ProductType/All");
        }

        [HttpGet("/Administration/ProductType/Delete/{productTypeId}")]
        public async Task<IActionResult> Delete(int productTypeId)
        {
            if(productTypeId <= 0)
            {
                return BadRequest();
            }

            bool productTypeIsDeleted = await this.productTypeService
                .DeleteTypeByIdAsync(productTypeId);

            return Redirect("/Administration/ProductType/All");
        }

        //TODO implement Edit
    }
}