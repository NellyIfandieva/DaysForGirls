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

    public class CategoryController : AdminController
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet("/Administration/Category/Create")]
        public async Task<IActionResult> Create()
        {
            await Task.Delay(0);
            return View();
        }

        [HttpPost("/Administration/Category/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateInputModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            var categoryServiceModel = new CategoryServiceModel
            {
                Name = model.Name,
                Description = model.Description
            };

            var createResult = await this.categoryService
                .CreateAsync(categoryServiceModel);

            if(createResult == null)
            {
                return Redirect("/Home/Error");
            }

            return Redirect("/Administration/Category/All");
        }

        [HttpGet("/Administration/Category/All")]
        public async Task<IActionResult> All()
        {
            var allCategories = await this.categoryService
                .DisplayAll();

            var viewModels = allCategories
                .Select(c => new CategoryDisplayAllViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    IsDeleted = c.IsDeleted
                })
                .OrderBy(c => c.Name);

            return View(viewModels);
        }

        [HttpGet("/Administration/Category/Edit/{categoryId}")]
        public async Task<IActionResult> Edit(int categoryId)
        {
            if (categoryId <= 0)
            {
                return Redirect("/Home/Error");
            }

            var categoryFromDb = await this.categoryService
                .GetCategoryByIdAsync(categoryId);

            if (categoryFromDb == null)
            {
                return Redirect("/Home/Error");
            }

            var categoryToEdit = new CategoryEditInputModel
            {
                Id = categoryFromDb.Id,
                Name = categoryFromDb.Name,
                Description = categoryFromDb.Description
            };

            return View(categoryToEdit);
        }

        [HttpPost("/Administration/Category/Edit/{categoryId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int categoryId, CategoryEditInputModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            var categoryToEdit = new CategoryServiceModel
            {
                Id = categoryId,
                Name = model.Name,
                Description = model.Description
            };

            var editResult = await this.categoryService
                .EditAsync(categoryToEdit);

            return Redirect("/Administration/Category/All");
        }

        [HttpGet("/Administration/Category/Delete/{categoryId}")]
        public async Task<IActionResult> Delete(int categoryId)
        {
            if (categoryId <= 0)
            {
                return Redirect("/Home/Error");
            }

            var categoryIsDeleted = await this.categoryService
                .DeleteCategoryByIdAsync(categoryId);

            if(categoryIsDeleted == null)
            {
                return Redirect("/Home/Error");
            }

            return Redirect("/Administration/Category/All");
        }
    }
}