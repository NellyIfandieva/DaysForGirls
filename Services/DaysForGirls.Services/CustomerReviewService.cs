using DaysForGirls.Data;
using DaysForGirls.Data.Models;
using DaysForGirls.Services.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DaysForGirls.Services
{
    public class CustomerReviewService : ICustomerReviewService
    {
        private readonly UserManager<DaysForGirlsUser> userManager;
        private readonly DaysForGirlsDbContext db;
        private readonly IProductService productService;

        public CustomerReviewService(
            UserManager<DaysForGirlsUser> userManager,
            DaysForGirlsDbContext db,
            IProductService productService)
        {
            this.userManager = userManager;
            this.db = db;
            this.productService = productService;
        }

        public async Task<bool> CreateAsync(CustomerReviewServiceModel model, int productId)
        {
            DaysForGirlsUser currentUser = 
                await this.userManager.FindByNameAsync(model.Author.Username);

            CustomerReview productReview = new CustomerReview
            {
                Title = model.Title,
                Text = model.Text,
                AuthorId = currentUser.Id,
                ProductId = model.Product.Id,
                CreatedOn = DateTime.UtcNow
            };

            this.db.CustomerReviews.Add(productReview);
            await this.db.SaveChangesAsync();

            bool reviewIsAddedToProduct = await this.productService.AddReviewToProductByProductIdAsync(productId, productReview.Id);

            return reviewIsAddedToProduct;
        }
    }
}
