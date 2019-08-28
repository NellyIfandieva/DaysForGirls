namespace DaysForGirls.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DaysForGirls.Data;
    using DaysForGirls.Data.Models;
    using DaysForGirls.Services.Models;
    using Microsoft.EntityFrameworkCore;

    public class CategoryService : ICategoryService
    {
        private readonly DaysForGirlsDbContext db;

        public CategoryService(DaysForGirlsDbContext db)
        {
            this.db = db;
        }

        public async Task<int> CreateAsync(CategoryServiceModel categoryServiceModel)
        {
            Category category = new Category
            {
                Name = categoryServiceModel.Name,
                Description = categoryServiceModel.Description
            };

            this.db.Categories.Add(category);
            int result = await db.SaveChangesAsync();

            int categoryId = category.Id;

            return categoryId;
        }

        public IQueryable<CategoryServiceModel> DisplayAll()
        {
            var allCategories = this.db.Categories
                .Select(c => new CategoryServiceModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    IsDeleted = c.IsDeleted
                });

            return allCategories;
        }

        public async Task<CategoryServiceModel> GetCategoryByIdAsync(int categoryId)
        {
            var categoryInDb = await this.db.Categories
                .SingleOrDefaultAsync(c => c.Id == categoryId);

            if(categoryInDb == null)
            {
                throw new ArgumentNullException(nameof(categoryInDb));
            }

            var categoryToReturn = new CategoryServiceModel
            {
                Id = categoryInDb.Id,
                Name = categoryInDb.Name,
                Description = categoryInDb.Description,
                IsDeleted = categoryInDb.IsDeleted
            };

            return categoryToReturn;
        }

        public async Task<bool> EditAsync(CategoryServiceModel model)
        {
            var categoryInDb = await this.db.Categories
                .SingleOrDefaultAsync(c => c.Id == model.Id);

            if (categoryInDb == null)
            {
                throw new ArgumentNullException(nameof(categoryInDb));
            }

            categoryInDb.Name = model.Name;
            categoryInDb.Description = model.Description;

            this.db.Update(categoryInDb);
            int result = await this.db.SaveChangesAsync();

            bool categoryIsEdited = result > 0;

            return categoryIsEdited;
        }

        public async Task<bool> DeleteCategoryByIdAsync(int categoryId)
        {
            var categoryToDelete = await this.db.Categories
                .SingleOrDefaultAsync(c => c.Id == categoryId);

            if (categoryToDelete == null)
            {
                throw new ArgumentNullException(nameof(categoryToDelete));
            }

            var productsInCategory = await this.db.Products
                .Where(p => p.Category.Name == categoryToDelete.Name)
                .ToListAsync();

            bool categoryIsDeleted = false;

            if(productsInCategory.Count() > 0)
            {
                return categoryIsDeleted;
            }
            else
            {
                this.db.Categories.Remove(categoryToDelete);
                int result = await this.db.SaveChangesAsync();
                categoryIsDeleted = result > 0;
            }

            return categoryIsDeleted;
        }
    }
}
