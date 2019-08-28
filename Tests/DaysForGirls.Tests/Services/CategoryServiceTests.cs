﻿namespace DaysForGirls.Tests.Services
{
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

    public class CategoryServiceTests
    {
        private ICategoryService categoryService;
        private List<Category> GetSampleCategories()
        {
            return new List<Category>()
            {
                new Category
                {
                    Name = "Wedding",
                    Description = "About the wedding day"
                },
                new Category
                {
                    Name = "Prom",
                    Description = "About the prom night"
                }
            };
        }

        private async Task SeedSampleCategories(DaysForGirlsDbContext db)
        {
            db.AddRange(GetSampleCategories());
            await db.SaveChangesAsync();
        }

        [Fact]
        public async Task Create_WithCorrectData_ShouldSuccessfullyCreate()
        {
            string errorMessagePrefix = "CategoryService CreateAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            this.categoryService = new CategoryService(db);

            CategoryServiceModel testCategory = new CategoryServiceModel
            {
                Name = "Accessory",
                Description = "Beautiful to the final touch"
            };

            int actualResult = await this.categoryService.CreateAsync(testCategory);
            Assert.True(actualResult > 0, errorMessagePrefix);
        }

        [Fact]
        public async Task GetById_WithExistingId_ShouldReturnACategory()
        {
            string errorMessagePrefix = "CategoryService GetCategoryByIdAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedSampleCategories(db);
            this.categoryService = new CategoryService(db);

            Category expectedData = db.Categories.First();

            CategoryServiceModel expectedDataServiceModel = new CategoryServiceModel
            {
                Id = expectedData.Id,
                Name = expectedData.Name,
                Description = expectedData.Description,
                IsDeleted = expectedData.IsDeleted
            };

            CategoryServiceModel actualData = await this.categoryService.GetCategoryByIdAsync(expectedDataServiceModel.Id);

            Assert.True(expectedDataServiceModel.Id == actualData.Id, errorMessagePrefix + " " + "Id is not returned properly.");
            Assert.True(expectedDataServiceModel.Name == actualData.Name, errorMessagePrefix + " " + "Name is not returned properly.");
            Assert.True(expectedDataServiceModel.Description == actualData.Description, errorMessagePrefix + " " + "Description is not returned properly");
            Assert.True(expectedDataServiceModel.IsDeleted == actualData.IsDeleted, errorMessagePrefix + " " + "IsDeleted is not returned properly.");
        }

        [Fact]
        public async Task GetById_WithNonexistentId_ShouldThrowArgumentNullException()
        {
            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedSampleCategories(db);
            this.categoryService = new CategoryService(db);

            await Assert.ThrowsAsync<ArgumentNullException>(() => this.categoryService.GetCategoryByIdAsync(8));
        }

        [Fact]
        public async Task DisplayAll_WithDummyData_ExpectedAllExistingCategoriesReturned()
        {
            string errorMessagePrefix = "CategoryService DisplayAll() method does not work properly.";
            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();

            await SeedSampleCategories(db);
            this.categoryService = new CategoryService(db);

            List<CategoryServiceModel> expectedResults = GetSampleCategories()
                .Select(c => new CategoryServiceModel
                {
                    Name = c.Name,
                    Description = c.Description,
                    IsDeleted = c.IsDeleted
                }).ToList();

            List<CategoryServiceModel> actualResults = await this.categoryService.DisplayAll().ToListAsync();
            

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedRecord = expectedResults[i];
                var actualRecord = actualResults[i];

                Assert.True(expectedRecord.Name == actualRecord.Name, errorMessagePrefix + " " + "Name is not returned properly.");
                Assert.True(expectedRecord.Description == actualRecord.Description, errorMessagePrefix + " " + "Description is not returned properly.");
                Assert.True(expectedRecord.IsDeleted == actualRecord.IsDeleted, errorMessagePrefix + " " + "IsDeleted is not returned properly.");
            }
        }

        [Fact]
        public async Task DisplayAll_WithZeroData_ExpectedAnEmptyList()
        {
            string errorMessagePrefix = "CategoryService DisplayAll() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            this.categoryService = new CategoryService(db);

            List<CategoryServiceModel> actualResults = await this.categoryService.DisplayAll().ToListAsync();

            Assert.True(actualResults.Count == 0, errorMessagePrefix);
        }

        [Fact]
        public async Task Edit_WithCorrectData_ExpectedCorrectlyEditedCategory()
        {
            string errorMessagePrefix = "CategoryService EditAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedSampleCategories(db);
            this.categoryService = new CategoryService(db);

            Category expectedData = db.Categories.First();
            CategoryServiceModel expectedServiceModel = new CategoryServiceModel
            {
                Id = expectedData.Id,
                Name = expectedData.Name,
                Description = expectedData.Description,
                IsDeleted = expectedData.IsDeleted
            };

            expectedServiceModel.Name = "New_Name";
            expectedServiceModel.Description = "New_Description";

            await this.categoryService.EditAsync(expectedServiceModel);

            Category actualData = db.Categories.First();

            var actualServiceModel = new CategoryServiceModel
            {
                Id = actualData.Id,
                Name = actualData.Name,
                Description = actualData.Description,
                IsDeleted = actualData.IsDeleted
            };

            Assert.True(actualServiceModel.Name == expectedServiceModel.Name, errorMessagePrefix + " " + "Name not edited properly.");
            Assert.True(actualServiceModel.Description == expectedServiceModel.Description, errorMessagePrefix + " " + "Description not edited properly.");
        }

        [Fact]
        public async Task DeleteCategoryById_WithExistingIdAndNoProducts_ExpectedToDeleteCategoryFromDb()
        {
            string errorMessagePrefix = "CategoryService DeleteByIdAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedSampleCategories(db);
            this.categoryService = new CategoryService(db);

            var categoryToDelete = db.Categories.First();

            bool actualResult = await this.categoryService.DeleteCategoryByIdAsync(categoryToDelete.Id);

            Assert.True(actualResult, errorMessagePrefix + " " + "Category was not deleted from the db");
        }

        [Fact]
        public async Task DeleteCategoryById_WithNonexistentId_ExpectedToThrowArgumentNullException()
        {
            string errorMessagePrefix = "CategoryService DeleteByIdAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedSampleCategories(db);
            this.categoryService = new CategoryService(db);

            await Assert.ThrowsAsync<ArgumentNullException>(() => this.categoryService.DeleteCategoryByIdAsync(8));
        }

        [Fact]
        public async Task DeleteCategoryById_WithExistingIdAndAProductRelatedToCategory_ExpectedToSetCategoryIsDeletedToTrue()
        {
            string errorMessagePrefix = "CategoryService DeleteByIdAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();

            var category = new Category
            {
                Name = "Mess",
                Description = "Big time"
            };

            db.Categories.Add(category);

            var product = new Product
            {
                Name = "Product One",
                Category = category,
                ProductType = new ProductType { Name = "Pants" },
                Manufacturer = new Manufacturer
                {
                    Name = "Manuf",
                    Description = "About Manuf",
                    Logo = new Logo { LogoUrl = "Manuf_logo"}
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

            this.categoryService = new CategoryService(db);

            var categoryToDelete = db.Categories.First();

            bool categoryIsDeletedSetToTrue = await this.categoryService
                .DeleteCategoryByIdAsync(categoryToDelete.Id);

            Assert.True(categoryIsDeletedSetToTrue, errorMessagePrefix + " " + "Service returned false");
            Assert.True(categoryToDelete.IsDeleted, errorMessagePrefix + " " + "Category IsDeleted not set to True");
        }
    }
}
