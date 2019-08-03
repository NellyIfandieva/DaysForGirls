using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaysForGirls.Services;
using DaysForGirls.Services.Models;
using DaysForGirls.Web.InputModels;
using Microsoft.AspNetCore.Authorization;
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
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CustomerReviewInputModel model)
        {
            if(this.User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Identity/Account/Login");
            }

            //TODO implement logic to create a new Review
            var newCustomerReview = new CustomerReviewServiceModel
            {

            };
            
            return View();
        }
    }
}