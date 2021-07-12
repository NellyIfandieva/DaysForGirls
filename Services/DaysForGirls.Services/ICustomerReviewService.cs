namespace DaysForGirls.Services
{
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICustomerReviewService
    {
        Task<int?> CreateAsync(CustomerReviewServiceModel model, int productId);

        Task<IEnumerable<CustomerReviewServiceModel>> GetAllCommentsOfProductByProductId(int productId);

        Task<int?> DeleteReviewByIdAsync(int reviewId);

        Task<IEnumerable<CustomerReviewServiceModel>> DisplayAll();
    }
}
