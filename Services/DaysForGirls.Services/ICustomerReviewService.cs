namespace DaysForGirls.Services
{
    using DaysForGirls.Services.Models;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ICustomerReviewService
    {
        Task<bool> CreateAsync(CustomerReviewServiceModel model, int productId);

        IQueryable<CustomerReviewServiceModel> GetAllCommentsOfProductByProductId(int productId);

        //Task<CustomerReviewServiceModel> GetReviewByIdAsync(int reviewId);

        Task<bool> DeleteReviewByIdAsync(int reviewId);

        IQueryable<CustomerReviewServiceModel> DisplayAll();
    }
}
