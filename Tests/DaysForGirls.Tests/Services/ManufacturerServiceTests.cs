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
    public class ManufacturerServiceTests
    {
        private IManufacturerService manufacturerService;
        private List<Manufacturer> GetSampleManufacturers()
        {
            return new List<Manufacturer>()
            {
                new Manufacturer
                {
                    Name = "ManufacturerOne",
                    Description = "ManufacturerOne description",
                    Logo = new Logo
                    {
                        LogoUrl = "ManufacturerOne_Logo"
                    }
                },
                new Manufacturer
                {
                    Name = "ManufacturerTwo",
                    Description = "ManufacturerTwo description",
                    Logo = new Logo
                    {
                        LogoUrl = "ManufacturerTwo_Logo"
                    }
                }
            };
        }

        private async Task SeedSampleManufacturers(DaysForGirlsDbContext db)
        {
            db.AddRange(GetSampleManufacturers());
            await db.SaveChangesAsync();
        }

        [Fact]
        public async Task Create_WithCorrectData_ExpectedToCreateAManufacturer()
        {
            string errorMessagePrefix = "ManufacturerService CreateAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            this.manufacturerService = new ManufacturerService(db);

            ManufacturerServiceModel testManufacturer = new ManufacturerServiceModel
            {
                Name = "ManufacturerThree",
                Description = "ManufacturerThree description",
                Logo = new LogoServiceModel
                {
                    LogoUrl = "ManufacturerThree_Logo"
                }
            };

            int actualResult = await this.manufacturerService.CreateAsync(testManufacturer);
            Assert.True(actualResult > 0, errorMessagePrefix);
        }

        [Fact]
        public async Task GetById_WithExistingId_ShouldReturnAManufacturer()
        {
            string errorMessagePrefix = "ManufacturerService GetManufacturerByIdAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedSampleManufacturers(db);

            this.manufacturerService = new ManufacturerService(db);

            Manufacturer expectedData = db.Manufacturers.First();

            ManufacturerServiceModel expectedDataServiceModel = new ManufacturerServiceModel
            {
                Id = expectedData.Id,
                Name = expectedData.Name,
                Description = expectedData.Description,
                Logo = new LogoServiceModel
                {
                    Id = expectedData.Logo.Id,
                    LogoUrl = expectedData.Logo.LogoUrl
                },
                IsDeleted = expectedData.IsDeleted
            };

            ManufacturerServiceModel actualData = await this.manufacturerService.GetManufacturerByIdAsync(expectedData.Id);

            Assert.True(expectedDataServiceModel.Id == actualData.Id, errorMessagePrefix + " " + "Id is not returned properly.");
            Assert.True(expectedDataServiceModel.Name == actualData.Name, errorMessagePrefix + " " + "Name is not returned properly.");
            Assert.True(expectedDataServiceModel.Description == actualData.Description, errorMessagePrefix + " " + "Description is not returned properly");
            Assert.True(expectedDataServiceModel.Logo.Id == actualData.Logo.Id, errorMessagePrefix + " " + "LogoId is not returned properly.");
            Assert.True(expectedDataServiceModel.Logo.LogoUrl == actualData.Logo.LogoUrl, errorMessagePrefix + " " + "LogoUrl is not returned properly.");
        }

        [Fact]
        public async Task GetById_WithNonexistentId_ShouldReturnNull()
        {
            string errorMessagePrefix = "ManufacturerService DisplayAll() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            this.manufacturerService = new ManufacturerService(db);

            var actualResult = await this.manufacturerService.GetManufacturerByIdAsync(8);

            Assert.True(actualResult == null, errorMessagePrefix + " " + "Returns a nonexistent manufacturer.");
        }

        [Fact]
        public async Task DisplayAll_WithDummyData_ExpectedToReturnAllExistingManufacturers()
        {
            string errorMessagePrefix = "ManufacturerService DisplayAll() method does not work properly.";
            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();

            await SeedSampleManufacturers(db);
            this.manufacturerService = new ManufacturerService(db);

            List<ManufacturerServiceModel> expectedResults = GetSampleManufacturers()
                .Select(m => new ManufacturerServiceModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    IsDeleted = m.IsDeleted,
                    Logo = new LogoServiceModel
                    {
                        Id = m.Logo.Id,
                        LogoUrl = m.Logo.LogoUrl
                    }
                }).ToList();

            List<ManufacturerServiceModel> actualResults = await this.manufacturerService.DisplayAll().ToListAsync();


            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedRecord = expectedResults[i];
                var actualRecord = actualResults[i];

                Assert.True(expectedRecord.Name == actualRecord.Name, errorMessagePrefix + " " + "Name is not returned properly.");
                Assert.True(expectedRecord.Description == actualRecord.Description, errorMessagePrefix + " " + "Description is not returned properly.");
                Assert.True(expectedRecord.IsDeleted == actualRecord.IsDeleted, errorMessagePrefix + " " + "IsDeleted is not returned properly.");
                Assert.True(expectedRecord.Logo.Id == actualRecord.Logo.Id, errorMessagePrefix + " " + "LogoId is not returned properly.");
                Assert.True(expectedRecord.Logo.LogoUrl == actualRecord.Logo.LogoUrl, errorMessagePrefix + " " + "LogoUrl is not returned properly.");
            }
        }

        [Fact]
        public async Task DisplayAll_WithZeroData_ExpectedAnEmptyList()
        {
            string errorMessagePrefix = "ManufacturerService DisplayAll() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            this.manufacturerService = new ManufacturerService(db);

            List<ManufacturerServiceModel> actualResults = await this.manufacturerService.DisplayAll().ToListAsync();

            Assert.True(actualResults.Count == 0, errorMessagePrefix);
        }

        [Fact]
        public async Task Edit_WithCorrectData_ExpectedCorrectlyEditedManufacturer()
        {
            string errorMessagePrefix = "ManufacturerService EditAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedSampleManufacturers(db);
            this.manufacturerService = new ManufacturerService(db);

            Manufacturer expectedData = db.Manufacturers.First();

            ManufacturerServiceModel expectedServiceModel = new ManufacturerServiceModel
            {
                Id = expectedData.Id,
                Name = expectedData.Name,
                Description = expectedData.Description,
                Logo = new LogoServiceModel
                {
                    LogoUrl = "New_ManufacturerLogo"
                },
                IsDeleted = expectedData.IsDeleted
            };

            expectedServiceModel.Name = "New_ManufacturerName";
            expectedServiceModel.Description = "New_ManufacturerDescription";

            await this.manufacturerService.EditAsync(expectedServiceModel);

            Manufacturer actualData = db.Manufacturers.First();

            var actualServiceModel = new ManufacturerServiceModel
            {
                Id = actualData.Id,
                Name = actualData.Name,
                Description = actualData.Description,
                Logo = new LogoServiceModel
                {
                    LogoUrl = "New_ManufacturerLogo"
                },
                IsDeleted = actualData.IsDeleted
            };

            Assert.True(actualServiceModel.Name == expectedServiceModel.Name, errorMessagePrefix + " " + "Name not edited properly.");
            Assert.True(actualServiceModel.Description == expectedServiceModel.Description, errorMessagePrefix + " " + "Description not edited properly.");
            Assert.True(actualServiceModel.Logo.LogoUrl == expectedServiceModel.Logo.LogoUrl, errorMessagePrefix + " " + "LogoUrl not edited properly.");
            Assert.True(actualServiceModel.IsDeleted == expectedServiceModel.IsDeleted, errorMessagePrefix + " " + "IsDeleted not edited properly.");
        }

        [Fact]
        public async Task DeleteManufacturerById_WithExistingIdAndNoProducts_ExpectedToDeleteManufacturerFromDb()
        {
            string errorMessagePrefix = "ManufacturerService DeleteTypeByIdAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedSampleManufacturers(db);
            this.manufacturerService = new ManufacturerService(db);

            var manufacturerToDelete = db.Manufacturers.First();

            bool actualResult = await this.manufacturerService.DeleteManufacturerByIdAsync(manufacturerToDelete.Id);

            Assert.True(actualResult, errorMessagePrefix + " " + "Manufacturer was not deleted from the db");
        }

        [Fact]
        public async Task DeleteManufacturerById_WithNonexistentId_ExpectedToReturnFalse()
        {
            string errorMessagePrefix = "ManufacturerService DeleteTypeByIdAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            this.manufacturerService = new ManufacturerService(db);

            bool actualResult = await this.manufacturerService.DeleteManufacturerByIdAsync(8);

            Assert.True(actualResult == false, errorMessagePrefix + " " + "Returns true");
        }

        [Fact]
        public async Task DeleteManufacturerById_WithExistingIdAndAProductRelatedToCategory_ExpectedToSetManufacturerIsDeletedToTrue()
        {
            string errorMessagePrefix = "ManufacturerService DeleteTypeByIdAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();

            var manufacturer = new Manufacturer
            {
                Name = "Armani",
                Description = "About Armani",
                Logo = new Logo { LogoUrl = "Armani_logo" }
            };

            db.Manufacturers.Add(manufacturer);

            var product = new Product
            {
                Name = "Product One",
                Category = new Category
                {
                    Name = "Haha",
                    Description = "Haha is great"
                },
                ProductType = new ProductType { Name = "Gashti" },
                Manufacturer = manufacturer,
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

            this.manufacturerService = new ManufacturerService(db);

            var manufacturerToDelete = db.Manufacturers.First();

            bool manufacturerIsDeletedSetToTrue = await this.manufacturerService
                .DeleteManufacturerByIdAsync(manufacturerToDelete.Id);

            Assert.True(manufacturerIsDeletedSetToTrue, errorMessagePrefix + " " + "Service returned false");
            Assert.True(manufacturerToDelete.IsDeleted, errorMessagePrefix + " " + "Manufacturer IsDeleted not set to True");
        }
    }
}
