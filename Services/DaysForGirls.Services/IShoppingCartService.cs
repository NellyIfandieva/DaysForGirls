using DaysForGirls.Data.Models;
using DaysForGirls.Services.Models;
using System;
using System.Threading.Tasks;

namespace DaysForGirls.Services
{
    public interface IShoppingCartService
    {
        Task<string> AddItemToCartCartAsync(string userId, ShoppingCartItemServiceModel model);

        Task<ShoppingCartServiceModel> GetCartByUserIdAsync(string userId);
    }
}
