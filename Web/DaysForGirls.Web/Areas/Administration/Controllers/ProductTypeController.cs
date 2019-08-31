namespace DaysForGirls.Web.Areas.Administration.Controllers
{
    using InputModels;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Services;
    using Services.Models;
    using System.Linq;
    using System.Threading.Tasks;
    using ViewModels;

    public class ProductTypeController : AdminController
    {
        private readonly IProductTypeService productTypeService;

        public ProductTypeController(IProductTypeService productTypeService)
        {
            this.productTypeService = productTypeService;
        }

        [HttpGet("/Administration/ProductType/Create")]
        public async Task<IActionResult> Create()
        {
            await Task.Delay(0);
            return View();
        }

        [HttpPost("/Administration/ProductType/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductTypeCreateInputModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            var pTServiceModel = new ProductTypeServiceModel
            {
                Name = model.Name
            };

            bool isCreated = await this.productTypeService
                .CreateAsync(pTServiceModel);

            if(isCreated == false)
            {
                return Redirect("/Home/Error");
            }

            return Redirect("/Administration/ProductType/All");
        }

        [HttpGet("/Administration/ProductType/All")]
        public async Task<IActionResult> All()
        {
            var allProductTypes = await this.productTypeService
                .DisplayAll()
                .Select(pT => new ProductTypeDisplayAllViewModel
                {
                    Id = pT.Id,
                    Name = pT.Name,
                    IsDeleted = pT.IsDeleted
                })
                .OrderBy(pt => pt.Name)
                .ToListAsync();

            return View(allProductTypes);
        }

        [HttpGet("/Administration/ProductType/Edit/{productTypeId}")]
        public async Task<IActionResult> Edit(int productTypeId)
        {
            if (productTypeId <= 0)
            {
                return Redirect("/Home/Error");
            }

            var productTypeFromDb = await this.productTypeService
                .GetProductTypeByIdAsync(productTypeId);

            if(productTypeFromDb == null)
            {
                return Redirect("/Home/Error");
            }

            var productTypeToEdit = new ProductTypeEditInputModel
            {
                Id = productTypeId,
                Name = productTypeFromDb.Name,
                IsDeleted = productTypeFromDb.IsDeleted
            };

            return View(productTypeToEdit);
        }

        [HttpPost("/Administration/ProductType/Edit/{productTypeId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int productTypeId, ProductTypeEditInputModel model)
        {
            if(ModelState.IsValid == false)
            {
                return View(model);
            }

            var productTypeWithEdits = new ProductTypeServiceModel
            {
                Id = productTypeId,
                Name = model.Name
            };

            bool productTypeIsEdited = await this.productTypeService
                .EditAsync(productTypeWithEdits);

            if(productTypeIsEdited == false)
            {
                return Redirect("/Home/Error");
            }

            return Redirect("/Administration/ProductType/All");
        }

        [HttpGet("/Administration/ProductType/Delete/{productTypeId}")]
        public async Task<IActionResult> Delete(int productTypeId)
        {
            if (productTypeId <= 0)
            {
                return Redirect("/Home/Error");
            }

            bool productTypeIsDeleted = await this.productTypeService
                .DeleteTypeByIdAsync(productTypeId);

            if(productTypeIsDeleted == false)
            {
                return Redirect("/Home/Error");
            }

            return Redirect("/Administration/ProductType/All");
        }
    }
}