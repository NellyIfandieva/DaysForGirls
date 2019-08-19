using System.Linq;
using System.Threading.Tasks;
using DaysForGirls.Data;
using DaysForGirls.Data.Models;
using DaysForGirls.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace DaysForGirls.Services
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly DaysForGirlsDbContext db;

        public ProductTypeService(DaysForGirlsDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> Create(ProductTypeServiceModel prTServiceModel)
        {
            ProductType productType = new ProductType
            {
                Name = prTServiceModel.Name
            };

            this.db.ProductTypes.Add(productType);
            int result = await db.SaveChangesAsync();

            return result == 1;
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

        public async Task<bool> DeleteTypeByIdAsync(int productTypeId)
        {
            var productTypeToDelete = await this.db.ProductTypes
                .SingleOrDefaultAsync(pT => pT.Id == productTypeId);

            productTypeToDelete.IsDeleted = true;

            this.db.Update(productTypeToDelete);
            int result = await this.db.SaveChangesAsync();

            bool productTypeIsDeleted = result > 0;

            return productTypeIsDeleted;
        }
    }
}
