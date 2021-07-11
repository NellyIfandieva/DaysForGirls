﻿namespace DaysForGirls.Services
{
    using DaysForGirls.Data;
    using DaysForGirls.Data.Models;
    using DaysForGirls.Services.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
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

        public async Task<bool> CreateAsync(CustomerReviewServiceModel model, int productId)
        {
            if(productId <= 0)
            {
                return false;
            }

            CustomerReview productReview = new CustomerReview
            {
                Title = model.Title,
                Text = model.Text,
                AuthorId = model.AuthorId,
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
                .Include(cR => cR.Author)
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
                });

            return allProductComments;
        }

        public async Task<bool> DeleteReviewByIdAsync(int reviewId)
        {
            var reviewToDelete = await this.db.CustomerReviews
                .SingleOrDefaultAsync(r => r.Id == reviewId);

            if (reviewToDelete == null)
            {
                return false;
            }

            reviewToDelete.IsDeleted = true;

            this.db.Update(reviewToDelete);
            int result = await this.db.SaveChangesAsync();

            bool reviewIsDeleted = result > 0;

            return reviewIsDeleted;
        }

        public IQueryable<CustomerReviewServiceModel> DisplayAll()
        {
            var allReviewsInDb = this.db.CustomerReviews
                .Include(cR => cR.Author)
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
                });

            return allReviewsInDb;
        }
    }
}
