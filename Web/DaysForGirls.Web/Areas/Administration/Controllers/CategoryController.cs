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
    public class CategoryController : AdminController
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet("/Administration/Category/Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("/Administration/Category/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateInputModel model)
        {
            if(ModelState.IsValid == false)
            {
                return View(model);
            }
            CategoryServiceModel categoryServiceModel = new CategoryServiceModel
            {
                Name = model.Name,
                Description = model.Description
            };

            int newCategoryId = await this.categoryService.CreateAsync(categoryServiceModel);

            return Redirect("/Administration/Category/All");
        }

        public async Task<IActionResult> All()
        {
            var allCategories = await this.categoryService
                .DisplayAll()
                .Select(c => new CategoryDisplayAllViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    IsDeleted = c.IsDeleted
                })
                .OrderBy(c => c.Name)
                .ToListAsync();

            if(allCategories == null)
            {
                return NotFound();
            }

            return View(allCategories);
        }

        [HttpGet("/Administration/Category/Edit/{categoryId}")]
        public async Task<IActionResult> Edit(int categoryId)
        {
            if(categoryId <= 0)
            {
                return BadRequest();
            }

            var categoryFromDb = await this.categoryService
                .GetCategoryByIdAsync(categoryId);

            var categoryToEdit = new CategoryEditInputModel
            {
                Id = categoryFromDb.Id,
                Name = categoryFromDb.Name,
                Description = categoryFromDb.Description
            };

            return View(categoryToEdit);
        }

        [HttpPost("/Administration/Category/Edit/{categoryId}")]
        public async Task<IActionResult> Edit(int categoryId, CategoryEditInputModel model)
        {
            if(ModelState.IsValid == false)
            {
                return View(model);
            }

            var categoryToEdit = new CategoryServiceModel
            {
                Id = categoryId,
                Name = model.Name,
                Description = model.Description
            };

            bool categoryIsEdited = await this.categoryService
                .EditAsync(categoryToEdit);

            return Redirect("/Administration/Category/All");
        }

        [HttpGet("/Administration/Category/Delete/{categoryId}")]
        public async Task<IActionResult> Delete(int categoryId)
        {
            if (categoryId <= 0)
            {
                return BadRequest();
            }

            bool categoryIsDeleted = await this.categoryService
                .DeleteCategoryByIdAsync(categoryId);

            return Redirect("/Administration/Category/All");
        }
    }
}