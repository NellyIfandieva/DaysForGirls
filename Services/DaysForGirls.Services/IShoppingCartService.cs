using DaysForGirls.Data.Models;
using DaysForGirls.Services.Models;
using System;
using System.Threading.Tasks;

namespace DaysForGirls.Services
{
    public interface IShoppingCartService
    {
        Task<bool> Add(ShoppingCartItemServiceModel model);
    }
}
