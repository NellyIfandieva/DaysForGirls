using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DaysForGirls.Services;
using DaysForGirls.Services.Models;
using DaysForGirls.Web.InputModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DaysForGirls.Web.Controllers
{
    public class CustomerReviewsController : Controller
    {
        private readonly ICustomerReviewService customerReviewService;

        public CustomerReviewsController(ICustomerReviewService customerReviewService)
        {
            this.customerReviewService = customerReviewService;
        }

        [HttpGet]
        public IActionResult Create(int productId)
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CustomerReviewInputModel model, int productId)
        {
            if(this.User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Identity/Account/Login");
            }

            string username = this.User.Identity.Name;

            
            //TODO implement logic to create a new Review
            var newCustomerReview = new CustomerReviewServiceModel
            {
                Title = model.Title,
                Text = model.Text,
                Author = new DaysForGirlsUserServiceModel
                {
                    UserName = username
                }
            };

            bool isCreated = await this.customerReviewService.CreateAsync(newCustomerReview, productId);
            
            return Redirect("/Products/Details/productId");
        }
    }
}