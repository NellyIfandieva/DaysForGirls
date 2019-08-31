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

        private List<Product> GetSampleProductsFoRsearch()
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
                    Logo = new Logo { LogoUrl = "Armani_Logo" }
                },
                Colour = "Green",
                Size = "Fit",
                OrderId = null,
                SaleId = "yes",
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
                    Name = "Ehaa",
                    Description = "Ehaa is another great one"
                },
                ProductType = new ProductType { Name = "Charshaf" },
                Manufacturer = new Manufacturer
                {
                    Name = "Versace",
                    Description = "About Versace",
                    Logo = new Logo { LogoUrl = "Versace_Logo" }
                },
                Colour = "Yellow",
                Size = "Small",
                OrderId = null,
                SaleId = null,
                ShoppingCart = null,
                Price = 20.00m,
                Quantity = new Quantity { AvailableItems = 1 },
                Pictures = new List<Picture>()
                {
                    new Picture{ PictureUrl = "a" },
                    new Picture{ PictureUrl = "b" }
                },
                Reviews = new List<CustomerReview>()
                {
                    new CustomerReview
                    {
                        Author = new DaysForGirlsUser
                        {
                            Address = "S",
                            Email = "userOne@hello.com",
                            FirstName = "Ani",
                            LastName = "Ivanova",
                            PasswordHash = "123",
                            PhoneNumber = "0888888888",
                            UserName = "Ancheto"
                        },
                        AuthorId = "1",
                        CreatedOn = DateTime.UtcNow.AddDays(-10),
                        Title = "Lovely!",
                        Text = "I just love it"
                    },
                    new CustomerReview
                    {
                        Author = new DaysForGirlsUser
                        {
                            Address = "A",
                            Email = "userTwo@hello.com",
                            FirstName = "Mimi",
                            LastName = "Todorova",
                            PasswordHash = "123",
                            PhoneNumber = "0888888889",
                            UserName = "Mims"
                        },
                        AuthorId = "2",
                        CreatedOn = DateTime.UtcNow.AddDays(-8),
                        Title = "Gorgeous",
                        Text = "More than I ever expected"
                    }
                }
            },

            new Product
            {
                Name = "Product Three",
                Category = new Category
                {
                    Name = "Mountain",
                    Description = "Mountains are beautiful"
                },
                ProductType = new ProductType { Name = "Semki" },
                Manufacturer = new Manufacturer
                {
                    Name = "Valentina",
                    Description = "About Valia",
                    Logo = new Logo { LogoUrl = "Valentina_Logo" }
                },
                Colour = "Red",
                Size = "Larger",
                OrderId = null,
                SaleId = null,
                ShoppingCart = null,
                Price = 20.00m,
                Quantity = new Quantity { AvailableItems = 1 },
                Pictures = new List<Picture>()
                {
                    new Picture{ PictureUrl = "a" },
                    new Picture{ PictureUrl = "b" }
                },
                Reviews = new List<CustomerReview>()
                {
                    new CustomerReview
                    {
                        Author = new DaysForGirlsUser
                        {
                            Address = "B",
                            Email = "userThree@hello.com",
                            FirstName = "Pepi",
                            LastName = "Hristov",
                            PasswordHash = "123",
                            PhoneNumber = "0888888898",
                            UserName = "Peps"
                        },
                        AuthorId = "3",
                        CreatedOn = DateTime.UtcNow.AddDays(-7),
                        Title = "Amazingly wonderful",
                        Text = "Everybody said I look great in it!"
                    },
                    new CustomerReview
                    {
                        Author = new DaysForGirlsUser
                        {
                            Address = "D",
                            Email = "amam@hello.com",
                            FirstName = "Misho",
                            LastName = "Petrov",
                            PasswordHash = "123",
                            PhoneNumber = "0888888989",
                            UserName = "Mishkata"
                        },
                        AuthorId = "4",
                        CreatedOn = DateTime.UtcNow.AddDays(-3),
                        Title = "The best ever",
                        Text = "I want to sleep in it"
                    }
                }
            }
        };
        }

        private async Task SeedSampleProductsForSearch(DaysForGirlsDbContext db)
        {
            db.Products.AddRange(GetSampleProductsFoRsearch());
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
        public async Task GetById_WithNonexistentProductId_ExpectedToReturnNull()
        {
            string errorMessagePrefix = "ProductService GetById() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            this.productService = new ProductService(db);

            var actualResult = await this.productService.GetProductByIdAsync(0);

            Assert.True(actualResult == null, errorMessagePrefix + " " + "Does not return null");
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

            Assert.True(actualResults.Count == 0, errorMessagePrefix + " " + "Returns results.");
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
            string errorMessagePrefix = "ProductService AddProductToShoppingCartAsync() method does not work properly.";
            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            this.productService = new ProductService(db);

            var actualResult = await this.productService.AddProductToShoppingCartAsync(0, null);

            Assert.True(actualResult == false, errorMessagePrefix + " " + "Does not return null.");
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
        public async Task RemoveProductFromShoppingCartAsync_WithNonexistentProductId_ExpectedToReturnFalse()
        {
            string errorMessagePrefix = "ProductService RemoveFromCart() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            this.productService = new ProductService(db);

            bool actualResult = await this.productService.RemoveProductFromShoppingCartAsync(0);

            Assert.True(actualResult == false, errorMessagePrefix + " " + "Returns true.");

        }

        [Fact]
        public async Task GetAllSearchResultsByCriteria_WithAllValid_ExpectedToReturnProducts()
        {
            string errorMessagePrefix = "ProductService GetAllSearchResultsByCriteria() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            this.productService = new ProductService(db);

            await SeedSampleProductsForSearch(db);

            string criteria = "A";
            string criteriaToLower = criteria.ToLower();

            var expectedResults = db.Products
                .Include(p => p.Category)
                .Include(p => p.Manufacturer)
                .Include(p => p.ProductType)
                .Include(p => p.Reviews)
                .Where(p => p.Category.Name.ToLower().Contains(criteriaToLower)
                || p.ProductType.Name.ToLower().Contains(criteriaToLower)
                || p.Manufacturer.Name.ToLower().Contains(criteriaToLower))
                .ToList();

            var actualResults = this.productService.GetAllSearchResultsByCriteria(criteriaToLower);

            Assert.True(expectedResults.Count() == actualResults.Count(), errorMessagePrefix + " " + "Count of results does not match");

            //List<ProductServiceModel> productsToReturn = new List<ProductServiceModel>();

            //foreach (var prod in actualResult)
            //{
            //    var prodServModel = new ProductServiceModel
            //    {
            //        Id = prod.Id,
            //        Name = prod.Name,
            //        Category = new CategoryServiceModel
            //        {
            //            Name = prod.Category.Name
            //        },
            //        ProductType = new ProductTypeServiceModel
            //        {
            //            Name = prod.ProductType.Name
            //        },
            //        Description = prod.Description,
            //        Pictures = prod.Pictures
            //            .Select(pic => new PictureServiceModel
            //            {
            //                Id = pic.Id,
            //                PictureUrl = pic.PictureUrl
            //            })
            //            .ToList(),
            //        Colour = prod.Colour,
            //        Size = prod.Size,
            //        Manufacturer = new ManufacturerServiceModel
            //        {
            //            Id = prod.Manufacturer.Id,
            //            Name = prod.Manufacturer.Name
            //        },
            //        Price = prod.Price,
            //        Quantity = new QuantityServiceModel
            //        {
            //            Id = prod.Quantity.Id,
            //            AvailableItems = prod.Quantity.AvailableItems
            //        },
            //        SaleId = prod.SaleId,
            //        ShoppingCartId = prod.ShoppingCartId,
            //        OrderId = prod.OrderId,
            //        Reviews = prod.Reviews
            //            .Select(r => new CustomerReviewServiceModel
            //            {
            //                Id = r.Id,
            //                Title = r.Title,
            //                Text = r.Text,
            //                CreatedOn = r.CreatedOn.ToString(),
            //                AuthorId = r.AuthorId
            //            })
            //            .ToList()
            //    };
            //}

            //Assert.True(productsToReturn.Count() == 2, errorMessagePrefix + " " + "and does not return the correct number of search results.");
        }

        [Fact]
        public async Task CalculateProductPriceAsync_WithAllValidDataAndProductInSale_ExpectedToReturnTheProductSalePrice()
        {
            string errorMessagePrefix = "ProductService CalculateProductPriceAsync() does not work correctly";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();

            this.productService = new ProductService(db);

            Product testProductOne = new Product
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
                SaleId = "yes",
                ShoppingCart = null,
                Price = 20.00m,
                Quantity = new Quantity { AvailableItems = 1 },
                Pictures = new List<Picture>()
                    {
                        new Picture{ PictureUrl = "a" },
                        new Picture{ PictureUrl = "b" }
                    }
            };

            db.Products.AddRange(testProductOne);
            await db.SaveChangesAsync();

            var productToCheck = db.Products.First();

            decimal originalProductPrice = productToCheck.Price;

            decimal actualResult = await this.productService.CalculateProductPriceAsync(productToCheck.Id);

            Assert.True(originalProductPrice > actualResult, errorMessagePrefix + " " + "Price does not return correctly.");
        }

        [Fact]
        public async Task CalculateProductPriceAsync_WithAllValidDataAndProductNotInSale_ExpectedToReturnTheOriginalProductPrice()
        {
            string errorMessagePrefix = "ProductService CalculateProductPriceAsync() does not work correctly";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();

            this.productService = new ProductService(db);

            Product testProductTwo = new Product
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
                    Logo = new Logo { LogoUrl = "Armani_Logo" }
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
            };

            db.Products.AddRange(testProductTwo);
            await db.SaveChangesAsync();

            var productToCheck = db.Products.First();

            decimal originalProductPrice = productToCheck.Price;

            decimal actualResult = await this.productService.CalculateProductPriceAsync(productToCheck.Id);

            Assert.True(originalProductPrice == actualResult, errorMessagePrefix + " " + "Price does not return correctly.");
        }
    }
}
