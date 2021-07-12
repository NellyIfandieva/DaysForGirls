namespace DaysForGirls.Services
{
    using DaysForGirls.Data.Models;
    using DaysForGirls.Services.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IOrderService
    {
        Task<OrderServiceModel> CreateAsync(DaysForGirlsUser user);

        Task<IEnumerable<OrderServiceModel>> DisplayAllOrdersOfUserAsync(string userId);

        Task<IEnumerable<OrderServiceModel>> DisplayAllOrdersToAdmin();

        Task<OrderServiceModel> GetOrderByIdAsync(string orderId);

        Task<int?> EditOrderStatusAsync(OrderServiceModel model);

        Task<bool> CheckIfOrderBelongsToUser(string orderId, string currentUserId);
    }
}
