namespace DaysForGirls.Services
{
    using DaysForGirls.Data.Models;
    using DaysForGirls.Services.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IOrderService
    {
        Task<OrderServiceModel> CreateAsync(DaysForGirlsUser user);

        Task<List<OrderServiceModel>> DisplayAllOrdersOfUserAsync(string userId);

        IQueryable<OrderServiceModel> DisplayAllOrdersToAdmin();

        Task<OrderServiceModel> GetOrderByIdAsync(string orderId);

        Task<bool> EditOrderStatusAsync(OrderServiceModel model);
    }
}
