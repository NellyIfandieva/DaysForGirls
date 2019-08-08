using DaysForGirls.Data.Models;
using DaysForGirls.Services.Models;
using System;
using System.Threading.Tasks;

namespace DaysForGirls.Services
{
    public interface IShoppingCartService
    {
        Task<bool> CreateCart(string userId, ShoppingCartItemServiceModel model);
    }
}
