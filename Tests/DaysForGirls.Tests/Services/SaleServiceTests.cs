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
                    Products = new List<Product>()
                },
                new Sale
                {
                    Title = "Sale Two",
                    EndsOn = DateTime.UtcNow.AddDays(35),
                    Picture = "saleTwo_picture",
                    Products = new List<Product>()
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

            List<SaleServiceModel> actualResults = await this.saleService.DisplayAll().ToListAsync();


            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedRecord = expectedResults[i];
                var actualRecord = actualResults[i];

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
                    IsActive = s.IsActive,
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
        public async Task GetById_WithExistingId_ShouldReturnACategory()
        {
            string errorMessagePrefix = "SaleService GetSaleByIdAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedSampleSales(db);
            this.saleService = new SaleService(db);

            Sale expectedData = db.Sales.First();

            SaleServiceModel expectedDataServiceModel = new SaleServiceModel
            {
                Title = expectedData.Title,
                EndsOn = expectedData.EndsOn,
                Picture = expectedData.Picture,
                Products = expectedData.Products
                .Select(p => new ProductServiceModel
                { }).ToList()
            };

            SaleServiceModel actualData = await this.saleService.GetSaleByIdAsync(expectedDataServiceModel.Id);

            Assert.True(expectedDataServiceModel.Title == actualData.Title, errorMessagePrefix + " " + "Title is not returned properly.");
            Assert.True(expectedDataServiceModel.EndsOn == actualData.EndsOn, errorMessagePrefix + " " + "EndsOn is not returned properly.");
            Assert.True(expectedDataServiceModel.Picture == actualData.Picture, errorMessagePrefix + " " + "Picture is not returned properly");
            Assert.True(expectedDataServiceModel.Products.Count() == actualData.Products.Count(), errorMessagePrefix + " " + "Products Count is not returned properly.");
        }

        [Fact]
        public async Task AddProductToSale_WithValidData_ExpectedToIncreaseSaleNumOfProductsByOne()
        {
            string errorMessagePrefix = "SaleService GetSaleByIdAsync() method does not work properly.";

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
            await SeedSampleSales(db);
            this.saleService = new SaleService(db);

            Sale saleToAddTo = db.Sales.First();
            Product productToAddToSale = db.Products.First();

            bool isAdded = await this.saleService.AddProductToSaleAsync(saleToAddTo.Id, productToAddToSale.Id);

            Assert.True(isAdded, errorMessagePrefix);
        }
    }
}
