namespace DaysForGirls.Web.Areas.Administration.Controllers
{
    using Services;
    using DaysForGirls.Web.ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

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
                .DisplayAll()
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
                })
                .ToListAsync();

            return View(allCustomerReviews);
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