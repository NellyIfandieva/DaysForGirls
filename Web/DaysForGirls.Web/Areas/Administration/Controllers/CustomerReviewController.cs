namespace DaysForGirls.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using System.Linq;
    using System.Threading.Tasks;
    using ViewModels;

    public class CustomerReviewController : Controller
    {
        private readonly ICustomerReviewService customerReviewService;

        public CustomerReviewController(ICustomerReviewService customerReviewService)
        {
            this.customerReviewService = customerReviewService;
        }

        [HttpGet("/Administration/CustomerReview/All")]
        public async Task<IActionResult> All()
        {
            var allCustomerReviews = await this.customerReviewService
                .DisplayAll();

           var viewModels = allCustomerReviews
                .Select(cR => new CustomerReviewAdminDisplayAllViewModel
                {
                    Id = cR.Id,
                    AuthorId = cR.AuthorId,
                    AuthorUsername = cR.AuthorUsername,
                    CreatedOn = cR.CreatedOn,
                    Title = cR.Title,
                    Text = cR.Text,
                    IsDeleted = cR.IsDeleted,
                    ProductId = cR.ProductId
                });

            return View(viewModels);
        }

        [HttpGet("/Administration/Delete/{reviewId}")]
        public async Task<IActionResult> Delete(int reviewId)
        {
            if(reviewId <= 0)
            {
                return Redirect("/Home/Error");
            }

            bool reviewIsDeleted = await this.customerReviewService
                .DeleteReviewByIdAsync(reviewId);

            if(reviewIsDeleted == false)
            {
                return Redirect("/Home/Error");
            }

            return Redirect("/Administration/All");
        }
    }
}