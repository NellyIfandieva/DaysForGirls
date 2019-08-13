using DaysForGirls.Services.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaysForGirls.Services
{
    public interface IAdminService
    {
        Task<int> CreateAsync(ProductServiceModel productServiceModel);

        IQueryable<AdminProductAllServiceModel> DisplayAll();

        Task<ProductServiceModel> GetProductByIdAsync(int productId);

        //IQueryable<ProductServiceModel> GetAllProductsByIds(List<int> productIds);

        Task<bool> DeleteProductByIdAsync(int productId);

        Task<bool> EditAsync(int productId, ProductServiceModel model);

        Task<bool> UploadNewPictureToProductAsync(int productId, string imageUrl);

        Task<bool> AddProductToSaleAsync(int productId, string saleId);

        Task<ProductServiceModel> GetProductByNameAsync(string productName);
    }
}
