namespace DaysForGirls.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Common;
    using DaysForGirls.Data;
    using DaysForGirls.Data.Models;
    using DaysForGirls.Services;
    using DaysForGirls.Services.Models;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class ProductTypeServiceTests
    {
        private IProductTypeService productTypeService;
        private List<ProductType> GetDummyData()
        {
            return new List<ProductType>()
            {
                new ProductType
                {
                    Name = "Dress"
                },
                new ProductType
                {
                    Name = "Suit"
                }
            };
        }

        private async Task SeedData(DaysForGirlsDbContext db)
        {
            db.AddRange(GetDummyData());
            await db.SaveChangesAsync();
        }

        [Fact]
        public async Task Create_WithCorrectData_ShouldSuccessfullyCreate()
        {
            string errorMessagePrefix = "ProductTypeService Create() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            this.productTypeService = new ProductTypeService(db);

            ProductTypeServiceModel testProductType = new ProductTypeServiceModel
            {
                Name = "Accessory"
            };

            bool actualResult = await this.productTypeService.Create(testProductType);
            Assert.True(actualResult, errorMessagePrefix);
        }

        [Fact]
        public async Task GetById_WithExistingId_ShouldReturnAProductType()
        {
            string errorMessagePrefix = "ProductTypeService GetProductTypeByIdAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedData(db);
            this.productTypeService = new ProductTypeService(db);

            ProductType expectedData = db.ProductTypes.First();
            ProductTypeServiceModel expectedDataServiceModel = new ProductTypeServiceModel
            {
                Id = expectedData.Id,
                Name = expectedData.Name,
                IsDeleted = expectedData.IsDeleted
            };
            ProductTypeServiceModel actualData = await this.productTypeService.GetProductTypeByIdAsync(expectedDataServiceModel.Id);

            Assert.True(expectedDataServiceModel.Id == actualData.Id, errorMessagePrefix + " " + "Id is not returned properly.");
            Assert.True(expectedDataServiceModel.Name == actualData.Name, errorMessagePrefix + " " + "Name is not returned properly.");
            Assert.True(expectedDataServiceModel.IsDeleted == actualData.IsDeleted, errorMessagePrefix + " " + "IsDeleted is not returned properly.");
        }

        [Fact]
        public async Task GetById_WithNonexistentId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "ProductService GetProductTypeByIdAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedData(db);
            this.productTypeService = new ProductTypeService(db);

            await Assert.ThrowsAsync<ArgumentNullException>(() => this.productTypeService.GetProductTypeByIdAsync(8));
        }


        [Fact]
        public async Task DisplayAll_WithDummyData_ExpectedResultAreCorrect()
        {
            string errorMessagePrefix = "ProductTypeService DisplayAll() method does not work properly.";
            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            
            await SeedData(db);
            this.productTypeService = new ProductTypeService(db);

            List<ProductTypeServiceModel> actualResults = await this.productTypeService.DisplayAll().ToListAsync();
            List<ProductTypeServiceModel> expectedResults = GetDummyData()
                .Select(pT => new ProductTypeServiceModel
                {
                    Name = pT.Name
                }).ToList();

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedRecord = expectedResults[i];
                var actualRecord = actualResults[i];

                Assert.True(expectedRecord.Name == actualRecord.Name, errorMessagePrefix + " " + "Name is not returned properly.");
            }
        }

        [Fact]
        public async Task DisplayAll_WithZeroData_ShouldReturnAnEmptyList()
        {
            string errorMessagePrefix = "ProductTypeService DisplayAll() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            this.productTypeService = new ProductTypeService(db);

            List<ProductTypeServiceModel> actualResults = await this.productTypeService.DisplayAll().ToListAsync();

            Assert.True(actualResults.Count == 0, errorMessagePrefix);
        }

        [Fact]
        public async Task Edit_WithCorrectData_ShouldEditProductTypeCorrectly()
        {
            string errorMessagePrefix = "ProductTypeService Edit() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedData(db);
            this.productTypeService = new ProductTypeService(db);

            ProductType expectedData = db.ProductTypes.First();
            ProductTypeServiceModel expectedServiceModel = new ProductTypeServiceModel
            {
                Id = expectedData.Id,
                Name = expectedData.Name
            };

            expectedServiceModel.Name = "New_Name";

            await this.productTypeService.EditAsync(expectedServiceModel);

            ProductType actualData = db.ProductTypes.First();

            var actualServiceModel = new ProductTypeServiceModel
            {
                Id = actualData.Id,
                Name = actualData.Name
            };

            Assert.True(actualServiceModel.Name == expectedServiceModel.Name, errorMessagePrefix + " " + "Name not edited properly.");
        }
    }
}
