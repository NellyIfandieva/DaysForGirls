namespace DaysForGirls.Tests.Services
{
    using Common;
    using DaysForGirls.Data;
    using DaysForGirls.Data.Models;
    using DaysForGirls.Services;
    using DaysForGirls.Services.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class ProductTypeServiceTests
    {
        private IProductTypeService productTypeService;

        private List<ProductType> GetSampleProductTypes()
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

        private async Task SeedProductTypes(DaysForGirlsDbContext db)
        {
            db.AddRange(GetSampleProductTypes());
            await db.SaveChangesAsync();
        }

        [Fact]
        public async Task Create_WithCorrectData_ShouldCreateAProductType()
        {
            string errorMessagePrefix = "ProductTypeService CreateAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            this.productTypeService = new ProductTypeService(db);

            var testProductType = new ProductTypeServiceModel
            {
                Name = "Accessory"
            };

            var actualResult = await this.productTypeService.CreateAsync(testProductType);
            Assert.True(actualResult != null, errorMessagePrefix);
        }

        [Fact]
        public async Task GetById_WithExistingId_ExpectedToReturnAProductType()
        {
            string errorMessagePrefix = "ProductTypeService GetProductTypeByIdAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedProductTypes(db);
            this.productTypeService = new ProductTypeService(db);

            var expectedData = db.ProductTypes.First();
            var expectedDataServiceModel = new ProductTypeServiceModel
            {
                Id = expectedData.Id,
                Name = expectedData.Name,
                IsDeleted = expectedData.IsDeleted
            };

            var actualData = await this.productTypeService
                .GetProductTypeByIdAsync(expectedDataServiceModel.Id);

            Assert.True(expectedDataServiceModel.Id == actualData.Id, errorMessagePrefix + " " + "Id is not returned properly.");
            Assert.True(expectedDataServiceModel.Name == actualData.Name, errorMessagePrefix + " " + "Name is not returned properly.");
            Assert.True(expectedDataServiceModel.IsDeleted == actualData.IsDeleted, errorMessagePrefix + " " + "IsDeleted is not returned properly.");
        }

        [Fact]
        public async Task GetById_WithNonexistentId_ExpectedToReturnNull()
        {
            string errorMessagePrefix = "ProductTypeService DisplayAll() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            this.productTypeService = new ProductTypeService(db);

            var actualResult = await this.productTypeService.GetProductTypeByIdAsync(0);

            Assert.True(actualResult == null, errorMessagePrefix + " " + "Returns a product type.");
        }


        [Fact]
        public async Task DisplayAll_WithDummyData_ExpectedToReturnAllExistingProductTypes()
        {
            string errorMessagePrefix = "ProductTypeService DisplayAll() method does not work properly.";
            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();

            await SeedProductTypes(db);
            this.productTypeService = new ProductTypeService(db);

            var actualResults = await this.productTypeService.DisplayAll();
            var expectedResults = GetSampleProductTypes()
                .Select(pT => new ProductTypeServiceModel
                {
                    Name = pT.Name
                }).ToList();

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedRecord = expectedResults[i];
                var actualRecord = actualResults.ElementAt(i);

                Assert.True(expectedRecord.Name == actualRecord.Name, errorMessagePrefix + " " + "Name is not returned properly.");
            }
        }

        [Fact]
        public async Task DisplayAll_WithZeroData_ExpectedAnEmptyList()
        {
            string errorMessagePrefix = "ProductTypeService DisplayAll() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            this.productTypeService = new ProductTypeService(db);

            var actualResults = await this.productTypeService.DisplayAll();

            Assert.True(actualResults.ToList().Count == 0, errorMessagePrefix);
        }

        [Fact]
        public async Task Edit_WithCorrectData_ExpectedToCorrectlyEditProductType()
        {
            string errorMessagePrefix = "ProductTypeService EditAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedProductTypes(db);
            this.productTypeService = new ProductTypeService(db);

            var expectedData = db.ProductTypes.First();
            var expectedServiceModel = new ProductTypeServiceModel
            {
                Id = expectedData.Id,
                Name = expectedData.Name
            };

            expectedServiceModel.Name = "New_Name";

            await this.productTypeService.EditAsync(expectedServiceModel);

            var actualData = db.ProductTypes.First();

            var actualServiceModel = new ProductTypeServiceModel
            {
                Id = actualData.Id,
                Name = actualData.Name
            };

            Assert.True(actualServiceModel.Name == expectedServiceModel.Name, errorMessagePrefix + " " + "Name not edited properly.");
        }

        [Fact]
        public async Task DeleteProductTypeById_WithExistingIdAndNoProducts_ExpectedToDeleteProductTypeFromDb()
        {
            string errorMessagePrefix = "ProductTypeService DeleteTypeByIdAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedProductTypes(db);
            this.productTypeService = new ProductTypeService(db);

            var productTypeToDelete = db.ProductTypes.First();

            var actualResult = await this.productTypeService
                .DeleteTypeByIdAsync(productTypeToDelete.Id);

            Assert.True(actualResult != null, errorMessagePrefix + " " + "ProductType was not deleted from the db");
        }

        [Fact]
        public async Task DeleteProductTypeById_WithNonexistentId_ExpectedToThrowArgumentNullException()
        {
            string errorMessagePrefix = "CategoryService EditAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedProductTypes(db);
            this.productTypeService = new ProductTypeService(db);

            var actualResult = await this.productTypeService.DeleteTypeByIdAsync(0);

            Assert.True(actualResult == null, errorMessagePrefix + " " + "Returns true.");

        }

        [Fact]
        public async Task DeleteProductTypeById_WithExistingIdAndAProductRelatedToCategory_ExpectedToSetProductTypeIsDeletedToTrue()
        {
            string errorMessagePrefix = "CategoryService EditAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();

            var productType = new ProductType
            {
                Name = "Love"
            };

            db.ProductTypes.Add(productType);

            var product = new Product
            {
                Name = "Product One",
                Category = new Category
                {
                    Name = "Haha",
                    Description = "Haha is great"
                },
                ProductType = productType,
                Manufacturer = new Manufacturer
                {
                    Name = "Manuf",
                    Description = "About Manuf",
                    Logo = new Logo { LogoUrl = "Manuf_logo" }
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

            this.productTypeService = new ProductTypeService(db);

            var productTypeToDelete = db.ProductTypes.First();

            var typeIsDeletedSetToTrue = await this.productTypeService
                .DeleteTypeByIdAsync(productTypeToDelete.Id);

            Assert.True(typeIsDeletedSetToTrue != null, errorMessagePrefix + " " + "Service returned false");
            Assert.True(productTypeToDelete.IsDeleted, errorMessagePrefix + " " + "ProductType IsDeleted not set to True");
        }
    }
}
