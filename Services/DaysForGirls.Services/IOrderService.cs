namespace DaysForGirls.Services
{
    using DaysForGirls.Services.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IOrderService
    {
        Task<OrderServiceModel> CreateAsync(string userId);

        Task<List<OrderServiceModel>> DisplayAllOrdersOfUser(string userName);

        IQueryable<OrderServiceModel> DisplayAllOrdersToAdminAsync();
    }
}
