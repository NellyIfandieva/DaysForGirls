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
    public class SaleServiceTests
    {
        private ISaleService saleService;
        private List<Sale> GetSampleSales()
        {
            return new List<Sale>()
            {
                new Sale
                {
                    Title = "Sale One",
                    EndsOn = DateTime.UtcNow.AddDays(35),
                    Picture = "saleOne_picture",
                    Products = new HashSet<Product>()
                },
                new Sale
                {
                    Title = "Sale Two",
                    EndsOn = DateTime.UtcNow.AddDays(35),
                    Picture = "saleTwo_picture",
                    Products = new HashSet<Product>()
                }
            };
        }

        private async Task SeedSampleSales(DaysForGirlsDbContext db)
        {
            db.AddRange(GetSampleSales());
            await db.SaveChangesAsync();
        }

        [Fact]
        public async Task Create_WithCorrectData_ShouldSuccessfullyCreate()
        {
            string errorMessagePrefix = "CategoryService CreateAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            this.saleService = new SaleService(db);

            SaleServiceModel testCategory = new SaleServiceModel
            {
                Title = "Sale Three",
                EndsOn = DateTime.UtcNow.AddDays(35),
                Picture = "saleThree_picture"
            };

            string actualResult = await this.saleService.CreateAsync(testCategory);
            Assert.True(actualResult != null, errorMessagePrefix);
        }

        [Fact]
        public async Task DisplayAll_WithSampleSales_ExpectedAllExistingActiveSalesReturned()
        {
            string errorMessagePrefix = "SaleService DisplayAll() method does not work properly.";
            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();

            await SeedSampleSales(db);
            this.saleService = new SaleService(db);

            List<SaleServiceModel> expectedResults = db.Sales
                .Where(s => s.IsActive == true)
                .Select(ss => new SaleServiceModel
                {
                    Id = ss.Id,
                    Title = ss.Title,
                    EndsOn = ss.EndsOn,
                    Picture = ss.Picture,
                    Products = ss.Products
                        .Select(p => new ProductServiceModel
                        {
                            Id = p.Id,
                            Name = p.Name
                        })
                        .ToList()
                }).ToList();

           var actualResults = await this.saleService
                .DisplayAll();


            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedRecord = expectedResults.ElementAt(i);
                var actualRecord = actualResults.ElementAt(i);

                Assert.True(expectedRecord.Title == actualRecord.Title, errorMessagePrefix + " " + "Title is not returned properly.");
                Assert.True(expectedRecord.EndsOn == actualRecord.EndsOn, errorMessagePrefix + " " + "EndsOn is not returned properly.");
                Assert.True(expectedRecord.Picture == actualRecord.Picture, errorMessagePrefix + " " + "Picture is not returned properly.");
                Assert.True(expectedRecord.Products.Count() == actualRecord.Products.Count(), errorMessagePrefix + " " + "Products Count does not display correctly");
            }
        }

        [Fact]
        public async Task DisplayAllAdmin_WithSampleSales_ExpectedAllExistingNonDeletedSalesReturned()
        {
            string errorMessagePrefix = "SaleService DisplayAllAdmin() method does not work properly.";
            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();

            await SeedSampleSales(db);
            this.saleService = new SaleService(db);

            List<SaleServiceModel> expectedResults = db.Sales
                .Where(s => s.IsActive == true)
                .Select(s => new SaleServiceModel
                {
                    Title = s.Title,
                    EndsOn = s.EndsOn,
                    Picture = s.Picture,
                    Products = s.Products
                    .Where(p => p.SaleId == s.Id)
                    .Select(p => new ProductServiceModel
                    {
                        Id = p.Id
                    })
                    .ToList()
                }).ToList();

            List<SaleServiceModel> actualResults = await this.saleService.DisplayAllAdmin().ToListAsync();

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedRecord = expectedResults[i];
                var actualRecord = actualResults[i];

                Assert.True(expectedRecord.Title == actualRecord.Title, errorMessagePrefix + " " + "Title is not returned properly.");
                Assert.True(expectedRecord.EndsOn == actualRecord.EndsOn, errorMessagePrefix + " " + "EndsOn is not returned properly.");
                Assert.True(expectedRecord.Picture == actualRecord.Picture, errorMessagePrefix + " " + "Picture is not returned properly.");
                Assert.True(expectedRecord.IsActive == actualRecord.IsActive, errorMessagePrefix + " " + "IsActive is not returned properly.");
                Assert.True(expectedRecord.Products.Count() == actualRecord.Products.Count(), errorMessagePrefix + " " + "Products Count does not display correctly");
            }
        }

        [Fact]
        public async Task GetById_WithExistingId_ShouldReturnASale()
        {
            string errorMessagePrefix = "SaleService GetSaleByIdAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedSampleSales(db);
            this.saleService = new SaleService(db);

            Sale expectedData = db.Sales.First();

            SaleServiceModel expectedDataServiceModel = new SaleServiceModel
            {
                Id = expectedData.Id,
                Title = expectedData.Title,
                EndsOn = expectedData.EndsOn,
                Picture = expectedData.Picture,
                Products = new List<ProductServiceModel>()
            };

            SaleServiceModel actualData = await this.saleService.GetSaleByIdAsync(expectedDataServiceModel.Id);

            Assert.True(expectedDataServiceModel.Title == actualData.Title, errorMessagePrefix + " " + "Title is not returned properly.");
            Assert.True(expectedDataServiceModel.EndsOn == actualData.EndsOn, errorMessagePrefix + " " + "EndsOn is not returned properly.");
            Assert.True(expectedDataServiceModel.Picture == actualData.Picture, errorMessagePrefix + " " + "Picture is not returned properly");
            Assert.True(expectedDataServiceModel.Products.Count() == actualData.Products.Count(), errorMessagePrefix + " " + "Products Count is not returned properly.");
        }

        [Fact]
        public async Task GetById_WithNonexistingId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "SaleService GetSaleByTitleAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            this.saleService = new SaleService(db);

            var actualResult = await this.saleService.GetSaleByIdAsync(null);

            Assert.True(actualResult == null, errorMessagePrefix + " " + "Returns a nonexistent sale.");
        }

        [Fact]
        public async Task GetByTitle_WithExistingTitle_ShouldReturnASale()
        {
            string errorMessagePrefix = "SaleService GetSaleByTitleAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedSampleSales(db);
            this.saleService = new SaleService(db);

            Sale expectedData = db.Sales.First();

            SaleServiceModel expectedDataServiceModel = new SaleServiceModel
            {
                Id = expectedData.Id,
                Title = expectedData.Title,
                EndsOn = expectedData.EndsOn,
                Picture = expectedData.Picture,
                Products = new List<ProductServiceModel>()
            };

            SaleServiceModel actualData = await this.saleService.GetSaleByTitleAsync(expectedDataServiceModel.Title);

            Assert.True(expectedDataServiceModel.Title == actualData.Title, errorMessagePrefix + " " + "Title is not returned properly.");
            Assert.True(expectedDataServiceModel.EndsOn == actualData.EndsOn, errorMessagePrefix + " " + "EndsOn is not returned properly.");
            Assert.True(expectedDataServiceModel.Picture == actualData.Picture, errorMessagePrefix + " " + "Picture is not returned properly");
            Assert.True(expectedDataServiceModel.Products.Count() == actualData.Products.Count(), errorMessagePrefix + " " + "Products Count is not returned properly.");
        }

        [Fact]
        public async Task GetByTitle_WithNonexistingTitle_ShouldReturnNull()
        {
            string errorMessagePrefix = "SaleService GetByTitle() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            this.saleService = new SaleService(db);

            var actualResult = await this.saleService.GetSaleByTitleAsync("sale");

            Assert.True(actualResult == null, errorMessagePrefix + " " + "Returns nonexistent sale");
        }

        [Fact]
        public async Task AddProductToSale_WithValidData_ExpectedToIncreaseSaleNumOfProductsByOne()
        {
            string errorMessagePrefix = "SaleService AddProductToSale() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();

            Product product = new Product
            {
                Name = "Product One",
                Description = "Product One Description",
                Category = new Category
                {
                    Name = "Wedding",
                    Description = "About the wedding"
                },
                ProductType = new ProductType
                {
                    Name = "Dress"
                },
                Colour = "White",
                Size = "M",
                Manufacturer = new Manufacturer
                {
                    Name = "Versace",
                    Description = "Italiano",
                    Logo = new Logo { LogoUrl = "Versace_Logo" }
                },
                Pictures = new List<Picture>()
                    {
                        new Picture
                        {
                            PictureUrl = "Versace_1"
                        },
                        new Picture
                        {
                            PictureUrl = "Versace_2"
                        }
                    },
                Price = 100.50M,
                Quantity = new Quantity { AvailableItems = 1 }
            };
            db.Products.Add(product);
            await SeedSampleSales(db);
            this.saleService = new SaleService(db);

            Sale saleToAddTo = db.Sales.First();
            Product productToAddToSale = db.Products.First();

            bool isAdded = await this.saleService.AddProductToSaleAsync(saleToAddTo.Id, productToAddToSale.Id);

            Assert.True(isAdded, errorMessagePrefix);
        }

        [Fact]
        public async Task AddProductToSale_WithNonexistentSaleId_ExpectedToReturnFalse()
        {
            string errorMessagePrefix = "SaleService AddProductToSale() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();

            Product product = new Product
            {
                Name = "Product One",
                Description = "Product One Description",
                Category = new Category
                {
                    Name = "Wedding",
                    Description = "About the wedding"
                },
                ProductType = new ProductType
                {
                    Name = "Dress"
                },
                Colour = "White",
                Size = "M",
                Manufacturer = new Manufacturer
                {
                    Name = "Versace",
                    Description = "Italiano",
                    Logo = new Logo { LogoUrl = "Versace_Logo" }
                },
                Pictures = new List<Picture>()
                    {
                        new Picture
                        {
                            PictureUrl = "Versace_1"
                        },
                        new Picture
                        {
                            PictureUrl = "Versace_2"
                        }
                    },
                Price = 100.50M,
                Quantity = new Quantity { AvailableItems = 1 }
            };
            db.Products.Add(product);
            await db.SaveChangesAsync();

            this.saleService = new SaleService(db);

            var productToAdd = db.Products.First();

            bool actualResult = await this.saleService.AddProductToSaleAsync(null, productToAdd.Id);

            Assert.True(actualResult == false, errorMessagePrefix + " " + "Returns true.");

        }

        [Fact]
        public async Task AddProductToSale_WithNonexistentProductId_ExpectedToThrowArgumentNullException()
        {
            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedSampleSales(db);

            this.saleService = new SaleService(db);

            var saleToAddTo = db.Sales.First();

            await Assert.ThrowsAsync<ArgumentNullException>(() => this.saleService.AddProductToSaleAsync(saleToAddTo.Id, 1));
        }

        [Fact]
        public async Task Edit_WithCorrectData_ExpectedCorrectlyEditedSale()
        {
            string errorMessagePrefix = "SaleService EditAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedSampleSales(db);
            this.saleService = new SaleService(db);

            Sale expectedData = db.Sales.First();

            SaleServiceModel expectedServiceModel = new SaleServiceModel
            {
                Id = expectedData.Id,
                Title = expectedData.Title,
                EndsOn = expectedData.EndsOn,
                Picture = expectedData.Picture
            };

            expectedServiceModel.Title = "New_SaleName";
            expectedServiceModel.Picture = "New_SalePicture";

            await this.saleService.EditAsync(expectedServiceModel);

            Sale actualData = db.Sales.First();

            var actualServiceModel = new SaleServiceModel
            {
                Id = actualData.Id,
                Title = actualData.Title,
                EndsOn = actualData.EndsOn,
                Picture = actualData.Picture
            };

            Assert.True(actualServiceModel.Title == expectedServiceModel.Title, errorMessagePrefix + " " + "Title not edited properly.");
            Assert.True(actualServiceModel.Picture == expectedServiceModel.Picture, errorMessagePrefix + " " + "Picture not edited properly.");
        }

        [Fact]
        public async Task Edit_WithNonexistentId_ShouldReturnFalse()
        {
            string errorMessagePrefix = "SaleService EditAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            this.saleService = new SaleService(db);

            bool actualResult = await this.saleService.EditAsync(null);

            Assert.True(actualResult == false, errorMessagePrefix + " " + "Edited sale.");
        }

        [Fact]
        public async Task DeleteSaleById_WithExistingIdAndNoProducts_ExpectedToDeleteSaleFromDb()
        {
            string errorMessagePrefix = "SaleService DeleteByIdAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedSampleSales(db);
            this.saleService = new SaleService(db);

            var saleToDelete = db.Sales.First();

            bool actualResult = await this.saleService.DeleteSaleById(saleToDelete.Id);

            Assert.True(actualResult, errorMessagePrefix + " " + "Sale was not deleted from the db");
        }

        [Fact]
        public async Task DeleteSaleById_WithNonexistentId_ExpectedToToReturnFalse()
        {
            string errorMessagePrefix = "SaleService DeleteByIdAsync() method does not work properly.";
            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedSampleSales(db);
            this.saleService = new SaleService(db);

            bool actualResult = await this.saleService.DeleteSaleById(null);

            Assert.True(actualResult == false, errorMessagePrefix + " " + "Returns true.");
        }
    }
}
