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
        public async Task<IActionResult> Create(CategoryCreateInputModel model)
        {
            CategoryServiceModel categoryServiceModel = new CategoryServiceModel
            {
                Name = model.Name,
                Description = model.Description
            };

            bool isCreated = await this.categoryService.Create(categoryServiceModel);

            return Redirect("/Administration/Category/All");
        }

        public async Task<IActionResult> All()
        {
            var allCategories = await this.categoryService
                .DisplayAll()
                .OrderBy(c => c.Name)
                .ToListAsync();

            var allCategoriesToDisplay = new HashSet<CategoryDisplayAllViewModel>();

            foreach(var category in allCategories)
            {
                CategoryDisplayAllViewModel vm = new CategoryDisplayAllViewModel
                {
                    Name = category.Name
                };

                allCategoriesToDisplay.Add(vm);
            }

            return View(allCategoriesToDisplay);
        }

        //TODO implement Details

        //TODO implement Delete
    }
}