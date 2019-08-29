namespace DaysForGirls.Web.Areas.Administration.Controllers
{
    using Services;
    using Services.Models;
    using InputModels;
    using ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;
    
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
            if(ModelState.IsValid == false)
            {
                return View(model);
            }

            var pTServiceModel = new ProductTypeServiceModel
            {
                Name = model.Name
            };

            bool isCreated = await this.productTypeService
                .CreateAsync(pTServiceModel);

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

            if(allProductTypes.Count() < 1)
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
    }
}