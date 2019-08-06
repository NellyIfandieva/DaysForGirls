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

        IQueryable<ProductServiceModel> AllWeddingAccessories();

        IQueryable<ProductServiceModel> AllPromProducts();

        IQueryable<ProductServiceModel> AllPromDresses();

        IQueryable<ProductServiceModel> AllPromSuits();

        IQueryable<ProductServiceModel> AllPromAccessories();

        IQueryable<ProductServiceModel> AllOtherProducts();

        IQueryable<ProductServiceModel> AllOtherDresses();

        IQueryable<ProductServiceModel> AllOtherSuits();

        IQueryable<ProductServiceModel> AllOtherAccessories();

        Task<bool> AddReviewToProductByProductIdAsync(int productId, int reviewId);

        IQueryable<ProductServiceModel> GetAllProductsByCategoryAndType(string productType, string category);
    }
}
