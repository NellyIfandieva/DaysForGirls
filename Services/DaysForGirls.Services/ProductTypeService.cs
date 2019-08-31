namespace DaysForGirls.Services
{
    using DaysForGirls.Data;
    using DaysForGirls.Data.Models;
    using DaysForGirls.Services.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductTypeService : IProductTypeService
    {
        private readonly DaysForGirlsDbContext db;

        public ProductTypeService(DaysForGirlsDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> CreateAsync(ProductTypeServiceModel prTServiceModel)
        {
            ProductType productType = new ProductType
            {
                Name = prTServiceModel.Name
            };

            this.db.ProductTypes.Add(productType);
            int result = await db.SaveChangesAsync();

            bool producTypeIsCreated = result > 0;

            return producTypeIsCreated;
        }

        public IQueryable<ProductTypeServiceModel> DisplayAll()
        {
            var allProductTypes = db.ProductTypes
                .Where(pb => pb.IsDeleted == false)
                .Select(pt => new ProductTypeServiceModel
                {
                    Id = pt.Id,
                    Name = pt.Name
                });

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

            var productTypeToReturn = new ProductTypeServiceModel
            {
                Id = productTypeInDb.Id,
                Name = productTypeInDb.Name,
                IsDeleted = productTypeInDb.IsDeleted
            };

            return productTypeToReturn;
        }

        public async Task<bool> EditAsync(ProductTypeServiceModel model)
        {
            var productTypeToEdit = await this.db.ProductTypes
                .SingleOrDefaultAsync(pT => pT.Id == model.Id);

            if (productTypeToEdit == null)
            {
                return false;
            }

            productTypeToEdit.Name = model.Name;

            this.db.Update(productTypeToEdit);
            int result = await this.db.SaveChangesAsync();

            bool productTypeIsEdited = result > 0;

            return productTypeIsEdited;
        }

        public async Task<bool> DeleteTypeByIdAsync(int productTypeId)
        {
            if(productTypeId <= 0)
            {
                return false;
            }

            var productTypeToDelete = await this.db.ProductTypes
                .SingleOrDefaultAsync(pT => pT.Id == productTypeId);

            if (productTypeToDelete == null)
            {
                return false;
            }

            var productsOfType = this.db.Products
                .Where(p => p.ProductTypeId == productTypeToDelete.Id);

            if (productsOfType.Count() > 0)
            {
                productTypeToDelete.IsDeleted = true;
                this.db.Update(productTypeToDelete);
            }
            else
            {
                this.db.ProductTypes.Remove(productTypeToDelete);
            }

            int result = await this.db.SaveChangesAsync();

            bool typeIsDeleted = result > 0;

            return typeIsDeleted;
        }
    }
}
