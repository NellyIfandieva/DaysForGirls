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
            if (productId <= 0)
            {
                return Redirect("/Home/Error");
            }

            if (this.User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Identity/Account/Login");
            }

            var product = await this.productService
                .GetProductByIdAsync(productId);

            if (product == null)
            {
                return Redirect("/Home/Error");
            }

            this.ViewData["productName"] = product.Name;

            CustomerReviewInputModel model = new CustomerReviewInputModel
            {
                ProductId = productId
            };  
            
            return View(model);
        }

        [HttpPost("/CustomerReviews/Create/{productId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerReviewInputModel model)
        {
            if(ModelState.IsValid == false)
            {
                return View(model);
            }

            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if(userId == null)
            {
                return Redirect("/Identity/Account/Login");
            }

            DaysForGirlsUser currentUser =
               await this.userManager.FindByIdAsync(userId);

            if(userId == null || currentUser == null)
            {
                return Redirect("/Home/Error");
            }

            var productId = model.ProductId;

            if(productId <= 0)
            {
                return Redirect("/Home/Error");
            }

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

            if(isCreated == false)
            {
                return Redirect("/Home/Error");
            }

            return Redirect("/Products/Details/" + productId);
        }
    }
}