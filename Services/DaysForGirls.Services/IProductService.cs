﻿namespace DaysForGirls.Services
{
    using DaysForGirls.Services.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IProductService
    {
        IQueryable<ProductDisplayAllServiceModel> DisplayAll();

        Task<ProductAsShoppingCartItem> GetProductByIdAsync(int productId);

        IQueryable<DisplayAllOfCategoryProductServiceModel> GetAllProductsOfCategory(string categoryName);

        IQueryable<DisplayAllOfCategoryAndTypeServiceModel> GetAllProductsOfTypeAndCategory(
            string productTypeName, string categoryName);

        Task<bool> AddProductToShoppingCartAsync(int productId, string shoppingCartId);

        Task<bool> RemoveProductFromShoppingCartAsync(int productId);

        List<ProductServiceModel> GetAllSearchResultsByCriteria(string criteria);

        Task<decimal> CalculateProductPriceAsync(int productId);
    }
}
