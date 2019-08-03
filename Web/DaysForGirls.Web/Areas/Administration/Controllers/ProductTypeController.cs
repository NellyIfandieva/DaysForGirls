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
        public async Task<IActionResult> Create(ProductTypeCreateInputModel model)
        {
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
                .OrderBy(pt => pt.Name)
                .ToListAsync();

            var productTypesToDisplay = new List<ProductTypeDisplayAllViewModel>();

            foreach (var prodType in allProductTypes)
            {
                ProductTypeDisplayAllViewModel vm = new ProductTypeDisplayAllViewModel
                {
                    Id = prodType.Id,
                    Name = prodType.Name
                };
                productTypesToDisplay.Add(vm);
            }

            return View(productTypesToDisplay);
        }
    }
}