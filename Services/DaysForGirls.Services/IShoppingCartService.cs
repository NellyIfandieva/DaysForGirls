namespace DaysForGirls.Services
{
    using DaysForGirls.Services.Models;
    using System.Threading.Tasks;

    public interface IShoppingCartService
    {
        Task<string> AddItemToCartCartAsync(string userId, ShoppingCartItemServiceModel model);

        Task<ShoppingCartServiceModel> GetCartByUserIdAsync(string userId);

        Task<bool> RemoveItemFromCartAsync(string userId, int itemId);
    }
}
