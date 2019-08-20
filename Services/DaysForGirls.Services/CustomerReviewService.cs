using DaysForGirls.Data;
using DaysForGirls.Data.Models;
using DaysForGirls.Services.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DaysForGirls.Services
{
    public class CustomerReviewService : ICustomerReviewService
    {
        private readonly UserManager<DaysForGirlsUser> userManager;
        private readonly DaysForGirlsDbContext db;

        public CustomerReviewService(
            UserManager<DaysForGirlsUser> userManager,
            DaysForGirlsDbContext db)
        {
            this.userManager = userManager;
            this.db = db;
        }

        public async Task<bool> CreateAsync(CustomerReviewServiceModel model, int productId)
        {
            DaysForGirlsUser currentUser = 
                await this.userManager.FindByNameAsync(model.AuthorUsername);

            CustomerReview productReview = new CustomerReview
            {
                Title = model.Title,
                Text = model.Text,
                AuthorId = currentUser.Id,
                Author = currentUser,
                ProductId = model.ProductId,
                CreatedOn = DateTime.UtcNow
            };

            this.db.CustomerReviews.Add(productReview);
            int result = await this.db.SaveChangesAsync();

            bool reviewIsAdded = result > 0;

            //bool reviewIsAddedToProduct = await this.productService.AddReviewToProductByProductIdAsync(productId, productReview.Id);

            return reviewIsAdded;
        }

        public IQueryable<CustomerReviewServiceModel> GetAllCommentsOfProductByProductId(int productId)
        {
            var allProductComments = this.db.CustomerReviews
                .Where(cR => cR.Product.Id == productId
                && cR.IsDeleted == false)
                .Select(cR => new CustomerReviewServiceModel
                {
                    Id = cR.Id,
                    Title = cR.Title,
                    Text = cR.Text,
                    CreatedOn = cR.CreatedOn.ToString("dddd, dd MMMM yyyy"),
                    AuthorUsername = cR.Author.UserName,
                    ProductId = productId
                });

            return allProductComments;
        }
    }
}
