namespace DaysForGirls.Services
{
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICustomerReviewService
    {
        Task<bool> CreateAsync(CustomerReviewServiceModel model, int productId);

        Task<IEnumerable<CustomerReviewServiceModel>> GetAllCommentsOfProductByProductId(int productId);

        Task<bool> DeleteReviewByIdAsync(int reviewId);

        Task<IEnumerable<CustomerReviewServiceModel>> DisplayAll();
    }
}
