using DaysForGirls.Data;
using DaysForGirls.Data.Models;
using DaysForGirls.Services;
using DaysForGirls.Services.Models;
using DaysForGirls.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DaysForGirls.Tests.Services
{
    public class CustomerReviewServiceTests
    {
        private ICustomerReviewService customerReviewService;

        private List<CustomerReview> GetSampleReviews()
        {
            return new List<CustomerReview>()
            {
                new CustomerReview
                {
                    Title = "Review One",
                    Text = "First review text",
                    AuthorId = "1",
                    ProductId = 1,
                    CreatedOn = DateTime.UtcNow
                },
                new CustomerReview
                {
                    Title = "Review Two",
                    Text = "Second review text",
                    AuthorId = "2",
                    ProductId = 2,
                    CreatedOn = DateTime.UtcNow
                }
            };
        }

        private async Task SeedSampleReviews(DaysForGirlsDbContext db)
        {
            db.AddRange(GetSampleReviews());
            await db.SaveChangesAsync();
        }

        [Fact]
        public async Task Create_WithCorrectData_ExpectedToCreateAReview()
        {
            string errorMessagePrefix = "CustomerReviewService CreateAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();

            this.customerReviewService = new CustomerReviewService(db);

            var newUser = new DaysForGirlsUser
            {
                UserName = "NELS",
                PasswordHash = "123",
                FirstName = "Nelly",
                LastName = "Ifandieva",
                Email = "nelly_vj@yahoo.com",
                Address = "s",
                PhoneNumber = "0888409104"
            };

            db.Users.Add(newUser);

            var product = new Product
            {
                Name = "Product",
                Description = "Descr",
                Category = new Category { Name = "Category" },
                ProductType = new ProductType { Name = "Type" },
                Colour = "Colour",
                Size = "Size",
                Manufacturer = new Manufacturer { Name = "Manufacturer" },
                Pictures = new List<Picture>()
                {
                    new Picture{ PictureUrl = "Picture One" },
                    new Picture{ PictureUrl = "Picture Two" }
                },
                Price = 500.00M,
                Quantity = new Quantity { AvailableItems = 1 },
                SaleId = null,
                OrderId = null
            };

            db.Products.Add(product);

            int result = await db.SaveChangesAsync();

            string userId = db.Users.First().Id;

            var testReview = new CustomerReviewServiceModel
            {
                Title = "Title",
                Text = "Text",
                AuthorId = userId,
                ProductId = product.Id,
                CreatedOn = DateTime.UtcNow.ToString("dddd, dd MMMM yyyy")
            };

            bool actualResult = await this.customerReviewService.CreateAsync(testReview, product.Id);
            Assert.True(actualResult, errorMessagePrefix);
        }

        [Fact]
        public async Task DisplayAll_WithDummyData_ExpectedToReturnAllReviewsByProductId()
        {
            string errorMessagePrefix = "CustomerReviewService GetAllCommentsOfProductByProductId() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();

            var product = new Product
            {
                Name = "Product",
                Description = "Descr",
                Category = new Category { Name = "Category" },
                ProductType = new ProductType { Name = "Type" },
                Colour = "Colour",
                Size = "Size",
                Manufacturer = new Manufacturer { Name = "Manufacturer" },
                Pictures = new List<Picture>()
                {
                    new Picture{ PictureUrl = "Picture One" },
                    new Picture{ PictureUrl = "Picture Two" }
                },
                Price = 500.00M,
                Quantity = new Quantity { AvailableItems = 1 },
                SaleId = null,
                OrderId = null
            };

            db.Products.Add(product);
            await db.SaveChangesAsync();

            var reviewOne = new CustomerReview
            {
                Title = "Review One",
                Text = "First review text",
                AuthorId = "1",
                ProductId = product.Id,
                CreatedOn = DateTime.UtcNow.AddDays(-2)
            };

            var reviewTwo = new CustomerReview
            {
                Title = "Review Two",
                Text = "Second review text",
                AuthorId = "2",
                ProductId = 2,
                CreatedOn = DateTime.UtcNow.AddDays(-5)
            };

            db.CustomerReviews.AddRange(reviewOne, reviewTwo);
            this.customerReviewService = new CustomerReviewService(db);

            var productId = db.Products.First().Id;

            List<CustomerReviewServiceModel> expectedResults = db.CustomerReviews
                .Where(cR => cR.ProductId == productId)
                .Select(cR => new CustomerReviewServiceModel
                {
                    Id = cR.Id,
                    Title = cR.Title,
                    Text = cR.Text,
                    AuthorId = cR.AuthorId,
                    ProductId = cR.ProductId,
                    CreatedOn = cR.CreatedOn.ToString("dddd, dd MMMM yyyy"),
                    IsDeleted = cR.IsDeleted
                }).ToList();

            var actualResults = await this
                .customerReviewService
                .GetAllCommentsOfProductByProductId(productId);


            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedRecord = expectedResults[i];
                var actualRecord = actualResults.ElementAt(i);

                Assert.True(expectedRecord.Title == actualRecord.Title, errorMessagePrefix + " " + "Title is not returned properly.");
                Assert.True(expectedRecord.Text == actualRecord.Text, errorMessagePrefix + " " + "Text is not returned properly.");
                Assert.True(expectedRecord.CreatedOn == actualRecord.CreatedOn, errorMessagePrefix + " " + "CreatedOn is not returned properly.");
                Assert.True(expectedRecord.AuthorId == actualRecord.AuthorId, errorMessagePrefix + " " + "AuthorUsername is not returned properly.");
                Assert.True(expectedRecord.ProductId == actualRecord.ProductId, errorMessagePrefix + " " + "ProductId is not returned properly.");
                Assert.True(expectedRecord.IsDeleted == actualRecord.IsDeleted, errorMessagePrefix + " " + "IsDeleted is not returned properly.");
            }
        }

        [Fact]
        public async Task DeleteReviewById_WithExistingReviewId_ExpectedToSetTheReviewIsDeletedToTrue()
        {
            string errorMessagePrefix = "CustomerReviewService GetAllCommentsOfProductByProductId() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            this.customerReviewService = new CustomerReviewService(db);

            var product = new Product
            {
                Name = "Product",
                Description = "Descr",
                Category = new Category { Name = "Category" },
                ProductType = new ProductType { Name = "Type" },
                Colour = "Colour",
                Size = "Size",
                Manufacturer = new Manufacturer { Name = "Manufacturer" },
                Pictures = new List<Picture>()
                {
                    new Picture{ PictureUrl = "Picture One" },
                    new Picture{ PictureUrl = "Picture Two" }
                },
                Price = 500.00M,
                Quantity = new Quantity { AvailableItems = 1 },
                SaleId = null,
                OrderId = null
            };

            db.Products.Add(product);
            int productAdded = await db.SaveChangesAsync();

            var reviewOne = new CustomerReview
            {
                Title = "Review One",
                Text = "First review text",
                AuthorId = "1",
                ProductId = product.Id,
                CreatedOn = DateTime.UtcNow.AddDays(-2)
            };

            db.CustomerReviews.Add(reviewOne);
            int reviewAdded = await db.SaveChangesAsync();

            int reviewId = db.CustomerReviews.First().Id;

            bool reviewIsDeleted = await this.customerReviewService.DeleteReviewByIdAsync(reviewId);

            Assert.True(reviewIsDeleted, errorMessagePrefix);
            Assert.True(reviewOne.IsDeleted, errorMessagePrefix + " " + "The review IsDeleted is not set to true");
        }
    }
}
