namespace DaysForGirls.Web.Controllers
{
    using Services;
    using Services.Models;
    using InputModels;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using System.Security.Claims;

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
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //string username = this.User.Identity.Name;
            var productId = model.ProductId;
            
            var newCustomerReview = new CustomerReviewServiceModel
            {
                Title = model.Title,
                Text = model.Text,
                AuthorId = userId,
                ProductId = model.ProductId
            };

            bool isCreated = await this.customerReviewService
                .CreateAsync(newCustomerReview, model.ProductId);

            return Redirect("/Products/Details/" + productId);
        }
    }
}