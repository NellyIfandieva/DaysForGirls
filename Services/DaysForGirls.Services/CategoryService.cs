using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaysForGirls.Data;
using DaysForGirls.Data.Models;
using DaysForGirls.Services.Models;

namespace DaysForGirls.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly DaysForGirlsDbContext db;

        public CategoryService(DaysForGirlsDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> Create(CategoryServiceModel categoryServiceModel)
        {
            Category category = new Category
            {
                Name = categoryServiceModel.Name,
                Description = categoryServiceModel.Description
            };

            this.db.Categories.Add(category);
            int result = await db.SaveChangesAsync();

            return result == 1;
        }

        public IQueryable<CategoryServiceModel> DisplayAll()
        {
            var allCategories = this.db.Categories
                .Where(ci => ci.IsDeleted == false)
                .Select(c => new CategoryServiceModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                });

            return allCategories;
        }
    }
}
