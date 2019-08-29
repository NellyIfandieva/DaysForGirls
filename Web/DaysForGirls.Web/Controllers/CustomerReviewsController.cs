namespace DaysForGirls.Web.Controllers
{
    using Services;
    using Services.Models;
    using InputModels;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class CustomerReviewsController : Controller
    {
        private readonly ICustomerReviewService customerReviewService;
        private readonly IProductService productService;

        public CustomerReviewsController(
            ICustomerReviewService customerReviewService,
            IProductService productService)
        {
            this.customerReviewService = customerReviewService;
            this.productService = productService;
        }

        [HttpGet("/CustomerReviews/Create/{productId}")]
        public async Task<IActionResult> Create(int productId)
        {
            this.ViewData["productId"] = productId;

            if (this.User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Identity/Account/Login");
            }

            await Task.Delay(0);
            return View();
        }

        [HttpPost("/CustomerReviews/Create/{productId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerReviewInputModel model)
        {
            string username = this.User.Identity.Name;
            var productId = model.ProductId;
            
            var newCustomerReview = new CustomerReviewServiceModel
            {
                Title = model.Title,
                Text = model.Text,
                AuthorUsername = username,
                ProductId = model.ProductId
            };

            bool isCreated = await this.customerReviewService
                .CreateAsync(newCustomerReview, model.ProductId);

            return Redirect("/Products/Details/" + productId);
        }
    }
}