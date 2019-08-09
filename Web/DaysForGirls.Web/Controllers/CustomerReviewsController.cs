using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DaysForGirls.Services;
using DaysForGirls.Services.Models;
using DaysForGirls.Web.InputModels;
using DaysForGirls.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DaysForGirls.Web.Controllers
{
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
            
            await Task.Delay(0);
            return View();
        }

        [Authorize]
        [HttpPost("/CustomerReviews/Create/{productId}")]
        public async Task<IActionResult> Create(CustomerReviewInputModel model)
        {
            if(this.User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Identity/Account/Login");
            }

            string username = this.User.Identity.Name;
            var productId = model.ProductId;
            
            //TODO implement logic to create a new Review
            var newCustomerReview = new CustomerReviewServiceModel
            {
                Title = model.Title,
                Text = model.Text,
                AuthorUsername = username,
                Product = new ProductServiceModel
                {
                    Id = model.ProductId
                }
            };

            bool isCreated = await this.customerReviewService.CreateAsync(newCustomerReview, model.ProductId);
            string id = productId.ToString();
            return Redirect("/Products/Details/{productId}");
        }
    }
}