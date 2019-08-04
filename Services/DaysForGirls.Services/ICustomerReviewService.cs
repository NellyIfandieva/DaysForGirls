using DaysForGirls.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DaysForGirls.Services
{
    public interface ICustomerReviewService
    {
        Task<bool> CreateAsync(CustomerReviewServiceModel model, int productId);
    }
}
