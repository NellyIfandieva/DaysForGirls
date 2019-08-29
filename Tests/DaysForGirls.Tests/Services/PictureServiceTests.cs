using DaysForGirls.Data;
using DaysForGirls.Data.Models;
using DaysForGirls.Services;
using DaysForGirls.Services.Models;
using DaysForGirls.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DaysForGirls.Tests.Services
{
    public class PictureServiceTests
    {
        private IPictureService pictureService;

        private List<Picture> GetSamplePictures()
        {
            return new List<Picture>()
            {
                new Picture
                {
                    PictureUrl = "PictureOne",
                    ProductId = 1
                },
                new Picture
                {
                    PictureUrl = "PictureTwo",
                    ProductId = 1
                }
            };
        }

        private async Task SeedSamplePictures(DaysForGirlsDbContext db)
        {
            db.Pictures.AddRange(GetSamplePictures());
            await db.SaveChangesAsync();
        }

        [Fact]
        public async Task GetById_WithExistingId_ShouldReturnAPicture()
        {
            string errorMessagePrefix = "PictureService GetPictureByIdAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedSamplePictures(db);

            this.pictureService = new PictureService(db);

            Picture expectedData = db.Pictures.First();

            var expectedDataServiceModel = new PictureServiceModel
            {
                Id = expectedData.Id,
                PictureUrl = expectedData.PictureUrl,
                ProductId = expectedData.ProductId
            };

            var actualData = await this.pictureService.GetPictureByIdAsync(expectedData.Id);

            Assert.True(expectedDataServiceModel.Id == actualData.Id, errorMessagePrefix + " " + "Id is not returned properly.");
            Assert.True(expectedDataServiceModel.PictureUrl == actualData.PictureUrl, errorMessagePrefix + " " + "PictureUrl is not returned properly.");
            Assert.True(expectedDataServiceModel.ProductId == actualData.ProductId, errorMessagePrefix + " " + "ProductId is not returned properly");
        }

        [Fact]
        public async Task GetById_WithNonexistentId_ShouldThrowArgumentNullException()
        {
            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedSamplePictures(db);

            this.pictureService = new PictureService(db);

            await Assert.ThrowsAsync<ArgumentNullException>(() => this.pictureService.GetPictureByIdAsync(8));
        }

        [Fact]
        public async Task GetPicsOfProductByProdId_WithValidData_ShouldReturnProductPictures()
        {
            string errorMessagePrefix = "PictureService GetPicturesOfProductByProductId() method does not work properly.";
            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();

            var product = new Product
            {
                Name = "Product One",
                Category = new Category
                {
                    Name = "Haha",
                    Description = "Haha is great"
                },
                ProductType = new ProductType { Name = "Gashti" },
                Manufacturer = new Manufacturer
                {
                    Name = "Armani",
                    Description = "About Armani",
                    Logo = new Logo { LogoUrl = "Armani_Logo" }
                },
                Colour = "Green",
                Size = "Fit",
                OrderId = null,
                SaleId = null,
                ShoppingCart = null,
                Price = 20.00m,
                Quantity = new Quantity { AvailableItems = 1 },
                Pictures = new List<Picture>()
                {
                    new Picture{ PictureUrl = "a" },
                    new Picture{ PictureUrl = "b" }
                }
            };

            db.Products.Add(product);
            await db.SaveChangesAsync();

            this.pictureService = new PictureService(db);

            int productId = db.Products.First().Id;

            var expectedPictures = db.Pictures
                .Where(pic => pic.ProductId == productId)
                .Select(pic => new PictureServiceModel
                {
                    Id = pic.Id,
                    PictureUrl = pic.PictureUrl
                })
                .ToList();

            var actualPictures = await this.pictureService
                .GetPicturesOfProductByProductId(productId)
                .Select(pic => new PictureServiceModel
                {
                    Id = pic.Id,
                    PictureUrl = pic.PictureUrl
                })
                .ToListAsync();

            Assert.True(expectedPictures.Count() == actualPictures.Count(), errorMessagePrefix + 
                " " + "Lists' Counts are not equal");

            for(int i = 0; i < expectedPictures.Count(); i++)
            {
                var expectedPic = expectedPictures[i];
                var actualPic = actualPictures[i];

                Assert.True(expectedPic.PictureUrl == actualPic.PictureUrl, errorMessagePrefix + 
                    " " + "PictureUrl do not return correctly.");
            }
        }

        [Fact]
        public async Task GetPicsOfProductByProdId_WithNonexistentProductId_ShouldThrowInvalidOperationException()
        {
            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedSamplePictures(db);

            this.pictureService = new PictureService(db);

            await Assert.ThrowsAsync<InvalidOperationException>(() => this.pictureService.GetPicturesOfProductByProductId(0)
            .Select(pic => new PictureServiceModel
            {
                Id = pic.Id,
                PictureUrl = pic.PictureUrl
            }).ToListAsync());
        }

        [Fact]
        public async Task DeletePicsOfDeletedProductByProdId_WithValidProdId_ShouldReturnTrue()
        {
            string errorMessagePrefix = "PictureService GetPicturesOfProductByProductId() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();

            var product = new Product
            {
                Name = "Product One",
                Category = new Category
                {
                    Name = "Haha",
                    Description = "Haha is great"
                },
                ProductType = new ProductType { Name = "Gashti" },
                Manufacturer = new Manufacturer
                {
                    Name = "Armani",
                    Description = "About Armani",
                    Logo = new Logo { LogoUrl = "Armani_Logo" }
                },
                Colour = "Green",
                Size = "Fit",
                OrderId = null,
                SaleId = null,
                ShoppingCart = null,
                Price = 20.00m,
                Quantity = new Quantity { AvailableItems = 1 },
                Pictures = new List<Picture>()
                {
                    new Picture{ PictureUrl = "a" },
                    new Picture{ PictureUrl = "b" }
                }
            };

            db.Products.Add(product);
            await db.SaveChangesAsync();

            this.pictureService = new PictureService(db);

            int productId = db.Products.First().Id;

            bool actualResult = await this.pictureService
                .DeletePicturesOfDeletedProductAsync(productId);

            Assert.True(actualResult, errorMessagePrefix + " " + "Service method returns false.");

            var actualPicturesAfterModification = db.Pictures
                .Where(pic => pic.ProductId == productId).ToList();

            foreach(var pic in actualPicturesAfterModification)
            {
                Assert.True(pic.IsDeleted, errorMessagePrefix + " " + 
                    "IsDeleted is not returned correctly.");
            }
        }

        [Fact]
        public async Task DeletePicsOfDeletedProductByProdId_WithNonexistentProdId_ShouldThrowInvalidOperationException()
        {
            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedSamplePictures(db);

            this.pictureService = new PictureService(db);

            await Assert.ThrowsAsync<InvalidOperationException>(() => this.pictureService.DeletePicturesOfDeletedProductAsync(0));
        }
    }
}
