using DaysForGirls.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaysForGirls.Services
{
    public interface IOrderService
    {
        Task<OrderServiceModel> CreateAsync(string userId);

        Task<List<OrderServiceModel>> DisplayAllOrdersOfUser(string userName);

        IQueryable<OrderServiceModel> DisplayAllOrdersToAdminAsync();
    }
}
