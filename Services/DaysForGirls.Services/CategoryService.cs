namespace DaysForGirls.Services
{
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Services.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CategoryService : ICategoryService
    {
        private readonly DaysForGirlsDbContext db;

        public CategoryService(DaysForGirlsDbContext db)
        {
            this.db = db;
        }

        public async Task<int> CreateAsync(CategoryServiceModel categoryServiceModel)
        {
            var category = new Category
            {
                Name = categoryServiceModel.Name,
                Description = categoryServiceModel.Description
            };

            this.db.Categories.Add(category);
            int result = await db.SaveChangesAsync();

            int categoryId = category.Id;

            return categoryId;
        }

        public async Task<IEnumerable<CategoryServiceModel>> DisplayAll()
        {
            var allCategories = await this.db
                .Categories
                .Select(c => new CategoryServiceModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    IsDeleted = c.IsDeleted
                })
                .ToListAsync();

            return allCategories;
        }

        public async Task<CategoryServiceModel> GetCategoryByIdAsync(int categoryId)
        {
            var categoryInDb = await this.db.Categories
                .SingleOrDefaultAsync(c => c.Id == categoryId);

            if (categoryInDb == null)
            {
                return null;
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
                return false;
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
            if(categoryId <= 0)
            {
                return false;
            }

            var categoryToDelete = await this.db
                .Categories
                .SingleOrDefaultAsync(c => c.Id == categoryId);

            if (categoryToDelete == null)
            {
                return false;
            }

            var productsInCategory = this.db
                .Products
                .Where(p => p.Category.Name == categoryToDelete.Name);

            if (productsInCategory.Any() == false)
            {
                categoryToDelete.IsDeleted = true;
                this.db.Update(categoryToDelete);
            }
            else
            {
                this.db.Categories.Remove(categoryToDelete);
            }

            int result = await this.db.SaveChangesAsync();

            bool categoryIsDeleted = result > 0;

            return categoryIsDeleted;
        }
    }
}
