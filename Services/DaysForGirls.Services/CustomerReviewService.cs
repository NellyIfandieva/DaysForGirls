namespace DaysForGirls.Services
{
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CustomerReviewService : ICustomerReviewService
    {
        private readonly DaysForGirlsDbContext db;

        public CustomerReviewService(
            DaysForGirlsDbContext db)
        {
            this.db = db;
        }

        public async Task<int?> CreateAsync(
            CustomerReviewServiceModel model, 
            int productId)
        {
            if(productId <= 0)
            {
                return null;
            }

            var productReview = new CustomerReview
            {
                Title = model.Title,
                Text = model.Text,
                AuthorId = model.AuthorId,
                ProductId = model.ProductId,
                CreatedOn = DateTime.UtcNow
            };

            this.db.CustomerReviews.Add(productReview);
            return await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<CustomerReviewServiceModel>> GetAllCommentsOfProductByProductId(int productId)
        {
            var allProductComments = await this.db
                .CustomerReviews
                .Where(cR => cR.Product.Id == productId
                        && cR.IsDeleted == false)
                .Select(cR => new CustomerReviewServiceModel
                {
                    Id = cR.Id,
                    Title = cR.Title,
                    Text = cR.Text,
                    CreatedOn = cR.CreatedOn.ToString("dddd, dd MMMM yyyy"),
                    AuthorId = cR.AuthorId,
                    AuthorUsername = cR.Author.UserName,
                    ProductId = productId
                }).ToListAsync();

            return allProductComments;
        }

        public async Task<int?> DeleteReviewByIdAsync(int reviewId)
        {
            var reviewToDelete = await this.db.CustomerReviews
                .SingleOrDefaultAsync(r => r.Id == reviewId);

            if (reviewToDelete == null)
            {
                return null;
            }

            reviewToDelete.IsDeleted = true;

            this.db.Update(reviewToDelete);
            return await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<CustomerReviewServiceModel>> DisplayAll()
        {
            var allReviewsInDb = await this.db
                .CustomerReviews
                .Select(cR => new CustomerReviewServiceModel
                {
                    Id = cR.Id,
                    AuthorUsername = cR.Author.UserName,
                    AuthorId = cR.AuthorId,
                    CreatedOn = cR.CreatedOn.ToString("dddd, dd MMMM yyyy"),
                    Title = cR.Title,
                    Text = cR.Text,
                    IsDeleted = cR.IsDeleted,
                    ProductId = cR.ProductId
                })
                .ToListAsync();

            return allReviewsInDb;
        }
    }
}
