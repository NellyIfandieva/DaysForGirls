namespace DaysForGirls.Web.Controllers
{
    using DaysForGirls.Data.Models;
    using InputModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using Services.Models;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class CustomerReviewsController : Controller
    {
        private readonly ICustomerReviewService customerReviewService;
        private readonly IProductService productService;
        private readonly UserManager<DaysForGirlsUser> userManager;

        public CustomerReviewsController(
            ICustomerReviewService customerReviewService,
            IProductService productService,
            UserManager<DaysForGirlsUser> userManager)
        {
            this.customerReviewService = customerReviewService;
            this.productService = productService;
            this.userManager = userManager;
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

            DaysForGirlsUser currentUser =
               await this.userManager.FindByIdAsync(userId);

            var productId = model.ProductId;

            var newCustomerReview = new CustomerReviewServiceModel
            {
                Title = model.Title,
                Text = model.Text,
                AuthorId = userId,
                AuthorUsername = currentUser.UserName,
                ProductId = model.ProductId
            };

            bool isCreated = await this.customerReviewService
                .CreateAsync(newCustomerReview, model.ProductId);

            return Redirect("/Products/Details/" + productId);
        }
    }
}