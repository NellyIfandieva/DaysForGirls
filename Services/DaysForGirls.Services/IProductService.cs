using DaysForGirls.Services.Models;
using System.Linq;
using System.Threading.Tasks;

namespace DaysForGirls.Services
{
    public interface IProductService
    {
        Task<int> Create(ProductServiceModel productServiceModel);

        IQueryable<ProductServiceModel> DisplayAll();

        Task<ProductServiceModel> GetDetailsOfProductByIdAsync(int Id);

        Task<bool> GetProductToDeleteById(int id);

        IQueryable<ProductServiceModel> AllWeddingProducts();

        IQueryable<ProductServiceModel> AllWeddingDresses();

        IQueryable<ProductServiceModel> AllWeddingSuits();

        IQueryable<AccessoryServiceModel> AllWeddingAccessories();

        IQueryable<ProductServiceModel> AllPromProducts();

        IQueryable<ProductServiceModel> AllPromDresses();

        IQueryable<ProductServiceModel> AllPromSuits();

        IQueryable<AccessoryServiceModel> AllPromAccessories();

        IQueryable<ProductServiceModel> AllOtherProducts();

        IQueryable<ProductServiceModel> AllOtherDresses();

        IQueryable<ProductServiceModel> AllOtherSuits();

        IQueryable<AccessoryServiceModel> AllOtherAccessories();
    }
}
