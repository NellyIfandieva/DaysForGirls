namespace DaysForGirls.Services
{
    using DaysForGirls.Data;
    using DaysForGirls.Data.Models;
    using DaysForGirls.Services.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

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
            /*DaysForGirlsUser currentUser = 
               await this.userManager.FindByNameAsync(model.AuthorUsername);*/

            CustomerReview productReview = new CustomerReview
            {
                Title = model.Title,
                Text = model.Text,
                AuthorId = model.AuthorId,
                //Author = currentUser,
                ProductId = model.ProductId,
                CreatedOn = DateTime.UtcNow
            };

            this.db.CustomerReviews.Add(productReview);
            int result = await this.db.SaveChangesAsync();

            bool reviewIsAdded = result > 0;

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
                    AuthorId = cR.AuthorId,
                    ProductId = productId
                });

            return allProductComments;
        }

        public async Task<bool> DeleteReviewByIdAsync(int reviewId)
        {
            var reviewToDelete = await this.db.CustomerReviews
                .SingleOrDefaultAsync(r => r.Id == reviewId);

            if(reviewToDelete == null)
            {
                throw new ArgumentNullException(nameof(reviewToDelete));
            }

            reviewToDelete.IsDeleted = true;

            this.db.Update(reviewToDelete);
            int result = await this.db.SaveChangesAsync();

            bool reviewIsDeleted = result > 0;

            return reviewIsDeleted;
        }
    }
}
