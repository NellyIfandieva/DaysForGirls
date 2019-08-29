namespace DaysForGirls.Tests.Services
{
    using Data;
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

    public class ProductServiceTests
    {
        private IProductService productService;

        private List<Product> GetSampleProducts()
        {
            return new List<Product>()
            {
                new Product
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
                        Logo = new Logo{ LogoUrl = "Armani_Logo"}
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
                },
                new Product
                {
                    Name = "Product Two",
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
                        Logo = new Logo{ LogoUrl = "Armani_Logo"}
                    },
                    Colour = "Yellow",
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
                }
            };
        }

        private async Task SeedSampleProducts(DaysForGirlsDbContext db)
        {
            db.Products.AddRange(GetSampleProducts());
            await db.SaveChangesAsync();
        }

        [Fact]
        public async Task GetById_WithExistingId_ShouldReturnAProduct()
        {
            string errorMessagePrefix = "ProductService GetProductByIdAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedSampleProducts(db);

            this.productService = new ProductService(db);

            Product expectedData = db.Products.First();

            var picture = expectedData.Pictures.ElementAt(0).PictureUrl;

            var expectedProduct = new ProductAsShoppingCartItem
            {
                Id = expectedData.Id,
                Name = expectedData.Name,
                Colour = expectedData.Colour,
                Size = expectedData.Size,
                MainPictureUrl = picture,
                Price = expectedData.Price,
                AvailableItems = expectedData.Quantity.AvailableItems,
                SaleId = expectedData.SaleId,
                ShoppingCartId = expectedData.ShoppingCartId
            };

            var actualData = await this.productService.GetProductByIdAsync(expectedData.Id);

            Assert.True(expectedProduct.Id == actualData.Id, errorMessagePrefix + " " + "Id is not returned properly.");
            Assert.True(expectedProduct.Name == actualData.Name, errorMessagePrefix + " " + "Name is not returned properly.");
            Assert.True(expectedProduct.Colour == actualData.Colour, errorMessagePrefix + " " + "Description is not returned properly");
            Assert.True(expectedProduct.Size == actualData.Size, errorMessagePrefix + " " + "LogoId is not returned properly.");
            Assert.True(expectedProduct.MainPictureUrl == actualData.MainPictureUrl, errorMessagePrefix + " " + "LogoUrl is not returned properly.");
            Assert.True(expectedProduct.Price == actualData.Price, errorMessagePrefix + " " + "LogoUrl is not returned properly.");
            Assert.True(expectedProduct.AvailableItems == actualData.AvailableItems, errorMessagePrefix + " " + "LogoUrl is not returned properly.");
            Assert.True(expectedProduct.SaleId == actualData.SaleId, errorMessagePrefix + " " + "LogoUrl is not returned properly.");
            Assert.True(expectedProduct.ShoppingCartId == actualData.ShoppingCartId, errorMessagePrefix + " " + "LogoUrl is not returned properly.");
        }

        [Fact]
        public async Task GetById_WithNonexistentProductId_ExpectedToReturnArgumentNullException()
        {
            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedSampleProducts(db);
            this.productService = new ProductService(db);

            await Assert.ThrowsAsync<ArgumentNullException>(() => this.productService.GetProductByIdAsync(8));
        }

        [Fact]
        public async Task DisplayAll_WithDummyData_ExpectedToReturnAllExistingProducts()
        {
            string errorMessagePrefix = "ProductService DisplayAll() method does not work properly.";
            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();

            await SeedSampleProducts(db);
            this.productService = new ProductService(db);

            List<ProductDisplayAllServiceModel> expectedResults = GetSampleProducts()
                .Select(p => new ProductDisplayAllServiceModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Picture = new PictureServiceModel
                    {
                        PictureUrl = p.Pictures.ElementAt(0).PictureUrl
                    },
                    AvailableItems = p.Quantity.AvailableItems,
                    SaleId = p.SaleId,
                    ShoppingCartId = p.ShoppingCartId,
                    OrderId = p.OrderId
                }).ToList();

            List<ProductDisplayAllServiceModel> actualResults = await this.productService.DisplayAll().ToListAsync();


            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedRecord = expectedResults[i];
                var actualRecord = actualResults[i];

                Assert.True(expectedRecord.Name == actualRecord.Name, errorMessagePrefix + " " + "Name is not returned properly.");
                Assert.True(expectedRecord.Price == actualRecord.Price, errorMessagePrefix + " " + "Description is not returned properly.");
                Assert.True(expectedRecord.Picture.PictureUrl == actualRecord.Picture.PictureUrl, errorMessagePrefix + " " + "IsDeleted is not returned properly.");
                Assert.True(expectedRecord.AvailableItems == actualRecord.AvailableItems, errorMessagePrefix + " " + "LogoId is not returned properly.");
                Assert.True(expectedRecord.SaleId == actualRecord.SaleId, errorMessagePrefix + " " + "LogoUrl is not returned properly.");
                Assert.True(expectedRecord.ShoppingCartId == actualRecord.ShoppingCartId, errorMessagePrefix + " " + "LogoUrl is not returned properly.");
                Assert.True(expectedRecord.OrderId == actualRecord.OrderId, errorMessagePrefix + " " + "LogoUrl is not returned properly.");
            }
        }

        [Fact]
        public async Task DisplayAll_WithZeroData_ExpectedAnEmptyList()
        {
            string errorMessagePrefix = "ProductService DisplayAll() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            this.productService = new ProductService(db);

            List<ProductDisplayAllServiceModel> actualResults = await this.productService.DisplayAll().ToListAsync();

            Assert.True(actualResults.Count == 0, errorMessagePrefix);
        }

        [Fact]
        public async Task GetAllProductsOfCategory_WithSampleDataAndValidCategoryName_ExpectedToReturnAllProductsOfCategory()
        {
            string errorMessagePrefix = "ProductService GetAllProductsOfCategory() method does not work properly.";
            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();

            await SeedSampleProducts(db);
            this.productService = new ProductService(db);

            string categoryName = db.Categories.First().Name;

            List<DisplayAllOfCategoryProductServiceModel> expectedResults = GetSampleProducts()
                .Select(p => new DisplayAllOfCategoryProductServiceModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Picture = new PictureServiceModel
                    {
                        PictureUrl = p.Pictures.ElementAt(0).PictureUrl
                    },
                    AvailableItems = p.Quantity.AvailableItems,
                    IsInSale = p.IsInSale,
                    SaleId = p.SaleId,
                    ShoppingCartId = p.ShoppingCartId,
                    OrderId = p.OrderId
                }).ToList();

            List<DisplayAllOfCategoryProductServiceModel> actualResults = await this.productService.GetAllProductsOfCategory(categoryName).ToListAsync();

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedRecord = expectedResults[i];
                var actualRecord = actualResults[i];

                Assert.True(expectedRecord.Name == actualRecord.Name, errorMessagePrefix + " " + "Name is not returned properly.");
                Assert.True(expectedRecord.Price == actualRecord.Price, errorMessagePrefix + " " + "Description is not returned properly.");
                Assert.True(expectedRecord.Picture.PictureUrl == actualRecord.Picture.PictureUrl, errorMessagePrefix + " " + "IsDeleted is not returned properly.");
                Assert.True(expectedRecord.AvailableItems == actualRecord.AvailableItems, errorMessagePrefix + " " + "LogoId is not returned properly.");
                Assert.True(expectedRecord.IsInSale == actualRecord.IsInSale, errorMessagePrefix + " " + "IsInSale is not returned correctly.");
                Assert.True(expectedRecord.SaleId == actualRecord.SaleId, errorMessagePrefix + " " + "LogoUrl is not returned properly.");
                Assert.True(expectedRecord.ShoppingCartId == actualRecord.ShoppingCartId, errorMessagePrefix + " " + "LogoUrl is not returned properly.");
                Assert.True(expectedRecord.OrderId == actualRecord.OrderId, errorMessagePrefix + " " + "LogoUrl is not returned properly.");
            }
        }

        [Fact]
        public async Task GetAllProductsOfCategory_WithZeroData_ExpectedAnEmptyList()
        {
            string errorMessagePrefix = "ProductService GetAllProductsOfCategory() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();

            this.productService = new ProductService(db);

            List<DisplayAllOfCategoryProductServiceModel> actualResults = await this.productService.GetAllProductsOfCategory("Haha").ToListAsync();

            Assert.True(actualResults.Count == 0, errorMessagePrefix);
        }

        [Fact]
        public async Task GetAllProductsOfTypeAndCategory_WithAllValid_ExpectedToReturnAllExistingProdOfTypeAndCat()
        {
            string errorMessagePrefix = "ProductService DisplayAll() method does not work properly.";
            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();

            await SeedSampleProducts(db);
            this.productService = new ProductService(db);

            string productTypeName = db.Products.First().ProductType.Name;
            string categoryName = db.Products.First().Category.Name;

            List<DisplayAllOfCategoryAndTypeServiceModel> expectedResults = GetSampleProducts()
                .Select(p => new DisplayAllOfCategoryAndTypeServiceModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Picture = new PictureServiceModel
                    {
                        PictureUrl = p.Pictures.ElementAt(0).PictureUrl
                    },
                    Price = p.Price,
                    AvailableItems = p.Quantity.AvailableItems,
                    SaleId = p.SaleId,
                    ShoppingCartId = p.ShoppingCartId,
                    OrderId = p.OrderId
                }).ToList();

            List<DisplayAllOfCategoryAndTypeServiceModel> actualResults = await this.productService.GetAllProductsOfTypeAndCategory(productTypeName, categoryName).ToListAsync();

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedRecord = expectedResults[i];
                var actualRecord = actualResults[i];

                Assert.True(expectedRecord.Name == actualRecord.Name, errorMessagePrefix + " " + "Name is not returned properly.");
                Assert.True(expectedRecord.Price == actualRecord.Price, errorMessagePrefix + " " + "Description is not returned properly.");
                Assert.True(expectedRecord.Picture.PictureUrl == actualRecord.Picture.PictureUrl, errorMessagePrefix + " " + "IsDeleted is not returned properly.");
                Assert.True(expectedRecord.AvailableItems == actualRecord.AvailableItems, errorMessagePrefix + " " + "LogoId is not returned properly.");
                Assert.True(expectedRecord.SaleId == actualRecord.SaleId, errorMessagePrefix + " " + "LogoUrl is not returned properly.");
                Assert.True(expectedRecord.ShoppingCartId == actualRecord.ShoppingCartId, errorMessagePrefix + " " + "LogoUrl is not returned properly.");
                Assert.True(expectedRecord.OrderId == actualRecord.OrderId, errorMessagePrefix + " " + "LogoUrl is not returned properly.");
            }
        }

        [Fact]
        public async Task GetAllProductsOfTypeAndCategory_WithZeroData_ExpectedAnEmptyList()
        {
            string errorMessagePrefix = "ProductService GetAllProductsOfCategory() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();

            this.productService = new ProductService(db);

            List<DisplayAllOfCategoryAndTypeServiceModel> actualResults = await this.productService.GetAllProductsOfTypeAndCategory("a", "b").ToListAsync();

            Assert.True(actualResults.Count == 0, errorMessagePrefix);
        }

        [Fact]
        public async Task AddProductToShoppingCartAsync_AllValid_ExpectedToUpdateTheProduct()
        {
            string errorMessagePrefix = "ProductService AddProductToShoppingCartAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedSampleProducts(db);

            this.productService = new ProductService(db);

            Product expectedData = db.Products.First();
            int expectedDataAvailableQuantity = expectedData.Quantity.AvailableItems;
            string shoppingCartId = "22021975";

            bool actualResult = await this.productService.AddProductToShoppingCartAsync(expectedData.Id, shoppingCartId);
            int actualDataAvalailabelQuantity = expectedData.Quantity.AvailableItems;

            Assert.True(actualResult, errorMessagePrefix + " " + "The method returned false");
            Assert.True(expectedData.ShoppingCartId == shoppingCartId, errorMessagePrefix + " " + 
                "ShoppingCartId not set properly.");
            Assert.True(expectedDataAvailableQuantity > actualDataAvalailabelQuantity, errorMessagePrefix + " " + 
                "AvalableQuantity not decreased.");
        }

        [Fact]
        public async Task AddProductToShoppingCartAsync_WithNonexistentProductId_ExpectedToReturnArgumentNullException()
        {
            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedSampleProducts(db);
            this.productService = new ProductService(db);

            await Assert.ThrowsAsync<ArgumentNullException>(() => this.productService.AddProductToShoppingCartAsync(8, "22021975"));
        }

        [Fact]
        public async Task RemoveProductFromShoppingCartAsync_WithValidData_ExpectedToSetCarrtIdToNullAndIncreaseProductAmount()
        {
            string errorMessagePrefix = "ProductService AddProductToShoppingCartAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedSampleProducts(db);

            this.productService = new ProductService(db);

            Product expectedData = db.Products.First();
            int expectedDataAvailableQuantity = expectedData.Quantity.AvailableItems;

            bool actualResult = await this.productService.RemoveProductFromShoppingCartAsync(expectedData.Id);
            int actualDataAvalailabelQuantity = expectedData.Quantity.AvailableItems;

            Assert.True(actualResult, errorMessagePrefix + " " + "The method returned false");
            Assert.True(expectedData.ShoppingCartId == null, errorMessagePrefix + " " +
                "ShoppingCartId not set properly.");
            Assert.True(expectedDataAvailableQuantity < actualDataAvalailabelQuantity, errorMessagePrefix + " " +
                "AvalableQuantity not increased.");
        }

        [Fact]
        public async Task RemoveProductFromShoppingCartAsync_WithNonexistentProductId_ExpectedToReturnArgumentNullException()
        {
            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedSampleProducts(db);
            this.productService = new ProductService(db);

            await Assert.ThrowsAsync<ArgumentNullException>(() => this.productService.RemoveProductFromShoppingCartAsync(8));
        }

        [Fact]
        public async Task GetAllSearchResultsByCriteria_WithAllValid_ExpectedToReturnProducts()
        {
            string errorMessagePrefix = "ProductService GetAllSearchResultsByCriteria() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedSampleProducts(db);

            this.productService = new ProductService(db);

            string dateFormat = "dddd, dd MMMM yyyy";

            var actualResult = this.productService.GetAllSearchResultsByCriteria("a");

            List<ProductServiceModel> productsToReturn = new List<ProductServiceModel>();

            foreach (var prod in actualResult)
            {
                var prodServModel = new ProductServiceModel
                {
                    Id = prod.Id,
                    Name = prod.Name,
                    Category = new CategoryServiceModel
                    {
                        Name = prod.Category.Name
                    },
                    ProductType = new ProductTypeServiceModel
                    {
                        Name = prod.ProductType.Name
                    },
                    Description = prod.Description,
                    Pictures = prod.Pictures
                        .Select(pic => new PictureServiceModel
                        {
                            Id = pic.Id,
                            PictureUrl = pic.PictureUrl
                        })
                        .ToList(),
                    Colour = prod.Colour,
                    Size = prod.Size,
                    Manufacturer = new ManufacturerServiceModel
                    {
                        //Id = prod.ManufacturerId,
                        Name = prod.Manufacturer.Name
                    },
                    Price = prod.Price,
                    Quantity = new QuantityServiceModel
                    {
                        //Id = prod.QuantityId,
                        AvailableItems = prod.Quantity.AvailableItems
                    },
                    SaleId = prod.SaleId,
                    ShoppingCartId = prod.ShoppingCartId,
                    OrderId = prod.OrderId,
                    Reviews = prod.Reviews
                        .Select(r => new CustomerReviewServiceModel
                        {
                            Id = r.Id,
                            Title = r.Title,
                            Text = r.Text,
                            //CreatedOn = r.CreatedOn.ToString(dateFormat),
                            AuthorId = r.AuthorId
                        })
                        .ToList()
                };
            }

            Assert.True(productsToReturn.Count() == 2);
        }
    }
}
