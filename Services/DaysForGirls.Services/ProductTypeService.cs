namespace DaysForGirls.Services
{
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductTypeService : IProductTypeService
    {
        private readonly DaysForGirlsDbContext db;

        public ProductTypeService(DaysForGirlsDbContext db)
        {
            this.db = db;
        }

        public async Task<int?> CreateAsync(ProductTypeServiceModel prTServiceModel)
        {
            var productType = new ProductType
            {
                Name = prTServiceModel.Name
            };

            this.db.ProductTypes.Add(productType);
            return await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductTypeServiceModel>> DisplayAll()
        {
            var allProductTypes = await db.ProductTypes
                .Select(pt => new ProductTypeServiceModel
                {
                    Id = pt.Id,
                    Name = pt.Name,
                    IsDeleted = pt.IsDeleted
                }).ToListAsync();

            return allProductTypes;
        }

        public async Task<ProductTypeServiceModel> GetProductTypeByIdAsync(int productTypeId)
        {
            if(productTypeId <= 0)
            {
                return null;
            }

            var productTypeInDb = await this.db.ProductTypes
                .SingleOrDefaultAsync(pT => pT.Id == productTypeId);

            if (productTypeInDb == null)
            {
                return null;
            }

            return new ProductTypeServiceModel
            {
                Id = productTypeInDb.Id,
                Name = productTypeInDb.Name,
                IsDeleted = productTypeInDb.IsDeleted
            };
        }

        public async Task<int?> EditAsync(ProductTypeServiceModel model)
        {
            var productTypeToEdit = await this.db.ProductTypes
                .SingleOrDefaultAsync(pT => pT.Id == model.Id);

            if (productTypeToEdit == null)
            {
                return null;
            }

            productTypeToEdit.Name = model.Name;

            this.db.Update(productTypeToEdit);
            return await db.SaveChangesAsync();
        }

        public async Task<int?> DeleteTypeByIdAsync(int productTypeId)
        {
            if(productTypeId <= 0)
            {
                return null;
            }

            var productTypeToDelete = await this.db.ProductTypes
                .SingleOrDefaultAsync(pT => pT.Id == productTypeId);

            if (productTypeToDelete == null)
            {
                return null;
            }

            var productsOfType = this.db.Products
                .Where(p => p.ProductTypeId == productTypeToDelete.Id);

            if (productsOfType.Any())
            {
                productTypeToDelete.IsDeleted = true;
                this.db.Update(productTypeToDelete);
            }
            else
            {
                this.db.ProductTypes.Remove(productTypeToDelete);
            }

            return await db.SaveChangesAsync();
        }
    }
}
