namespace DaysForGirls.Tests.Services
{
    using DaysForGirls.Data.Models;
    using DaysForGirls.Services;
    using DaysForGirls.Services.Models;
    using DaysForGirls.Tests.Common;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class ShoppingCartServiceTests
    {
        private IShoppingCartService shoppingCartService;
        private IProductService productService;

        [Fact]
        public async Task AddProductToShoppingCart_WithValidData_ExpectedToReturnShoppingCartId()
        {
            string errorMessagePrefix = "ShoppingCartService AddProductToShoppingCart() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            this.productService = new ProductService(db);
            this.shoppingCartService = new ShoppingCartService(db, productService);

            var testUser = new DaysForGirlsUser
            {
                FirstName = "Mary",
                LastName = "Johnson",
                PasswordHash = "123",
                PhoneNumber = "0888888888",
                UserName = "UserTwo",
                Email = "userOne@userOne.com",
                Address = "S"
            };

            db.Users.Add(testUser);
            await db.SaveChangesAsync();

            Product testProduct = new Product
            {
                Name = "Product Three",
                Description = "Product Three description",
                Colour = "Green",
                Size = "Medium",
                Category = new Category
                {
                    Name = "CatName"
                },
                ProductType = new ProductType
                {
                    Name = "ProductTypeName"
                },
                Manufacturer = new Manufacturer
                {
                    Name = "ManufacturerName"
                },
                Pictures = new List<Picture>()
                {
                    new Picture
                    {
                        PictureUrl = "Prod_Three_1"
                    }
                },
                Price = 220.00M,
                Quantity = new Quantity
                {
                    AvailableItems = 1
                }
            };

            db.Products.Add(testProduct);
            await db.SaveChangesAsync();

            var productInDb = db.Products.First();

            var productToAdd = await this.productService
                .GetProductByIdAsync(productInDb.Id);

            await this.productService.CalculateProductPriceAsync(productInDb.Id);

            var shoppingCartItem = new ShoppingCartItemServiceModel
            {
                Product = productToAdd,
                Quantity = 1
            };

            string userId = db.Users.First().Id;

            var testCart = new ShoppingCart
            {
                UserId = testUser.Id
            };

            db.ShoppingCarts.Add(testCart);
            await db.SaveChangesAsync();

            string cartId = await this.shoppingCartService
                .AddItemToCartCartAsync(userId, shoppingCartItem);

            Assert.True(cartId != null, errorMessagePrefix + " " + "Does not return the shoppingCartId properly");
        }

        [Fact]
        public async Task AddProductToShoppingCart_WithInValidUserId_ExpectedToReturnNull()
        {
            string errorMessagePrefix = "ShoppingCartService AddItemToCart() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            this.productService = new ProductService(db);
            this.shoppingCartService = new ShoppingCartService(db, productService);

            Product testProduct = new Product
            {
                Name = "Product Three",
                Description = "Product Three description",
                Colour = "Green",
                Size = "Medium",
                Category = new Category
                {
                    Name = "CatName"
                },
                ProductType = new ProductType
                {
                    Name = "ProductTypeName"
                },
                Manufacturer = new Manufacturer
                {
                    Name = "ManufacturerName"
                },
                Pictures = new List<Picture>()
                {
                    new Picture
                    {
                        PictureUrl = "Prod_Three_1"
                    }
                },
                Price = 220.00M,
                Quantity = new Quantity
                {
                    AvailableItems = 1
                }
            };

            db.Products.Add(testProduct);
            await db.SaveChangesAsync();

            var productInDb = db.Products.First();

            var productToAdd = await this.productService
                .GetProductByIdAsync(productInDb.Id);

            await this.productService.CalculateProductPriceAsync(productInDb.Id);

            var shoppingCartItem = new ShoppingCartItemServiceModel
            {
                Product = productToAdd,
                Quantity = 1
            };

            var testCart = new ShoppingCart
            {
                UserId = "8"
            };

            var actualResult = await this.shoppingCartService.AddItemToCartCartAsync(null, shoppingCartItem);

            Assert.True(actualResult == null, errorMessagePrefix + " " + "Does not return null.");
        }

        [Fact]
        public async Task GetCartByUserId_WithValidUserId_ExpectedToReturnAShopCartServiceModel()
        {
            string errorMessagePrefix = "ShoppingCartService GetCartByIdAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            this.productService = new ProductService(db);
            this.shoppingCartService = new ShoppingCartService(db, productService);

            var testUser = new DaysForGirlsUser
            {
                FirstName = "Mary",
                LastName = "Johnson",
                PasswordHash = "123",
                PhoneNumber = "0888888888",
                UserName = "UserTwo",
                Email = "userOne@userOne.com",
                Address = "S"
            };

            db.Users.Add(testUser);
            await db.SaveChangesAsync();

            var cartUser = db.Users.First();
            string userId = cartUser.Id;

            var testCart = new ShoppingCart
            {
                User = cartUser,
                UserId = userId,
                ShoppingCartItems = new List<ShoppingCartItem>()
            };

            db.ShoppingCarts.Add(testCart);
            await db.SaveChangesAsync();

            var expectedResult = db.ShoppingCarts.First();

            var actualResult = await this.shoppingCartService.GetCartByUserIdAsync(userId);

            Assert.True(expectedResult.ShoppingCartItems.Count() == actualResult.ShoppingCartItems.Count(), errorMessagePrefix + " " + "Does not return the expected cart items count currently");
        }

        [Fact]
        public async Task GetCartByUserId_WithInvalidUserId_ExpectedToReturnNull()
        {
            string errorMessagePrefix = "ShoppingCartService RemoveItemFromCartAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            this.productService = new ProductService(db);
            this.shoppingCartService = new ShoppingCartService(db, productService);

            var actualResult = await this.shoppingCartService.GetCartByUserIdAsync(null);

            Assert.True(actualResult == null, errorMessagePrefix + " " + "Does not return null.");
        }

        [Fact]
        public async Task RemoveItemFromCartAsync_WithAllValidData_ExpectedToReturnTrue()
        {
            string errorMessagePrefix = "ShoppingCartService RemoveItemFromCartAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            this.productService = new ProductService(db);
            this.shoppingCartService = new ShoppingCartService(db, productService);

            var testUser = new DaysForGirlsUser
            {
                FirstName = "Mary",
                LastName = "Johnson",
                PasswordHash = "123",
                PhoneNumber = "0888888888",
                UserName = "UserTwo",
                Email = "userOne@userOne.com",
                Address = "S"
            };

            db.Users.Add(testUser);
            await db.SaveChangesAsync();

            Product testProduct = new Product
            {
                Name = "Product Three",
                Description = "Product Three description",
                Colour = "Green",
                Size = "Medium",
                Category = new Category
                {
                    Name = "CatName"
                },
                ProductType = new ProductType
                {
                    Name = "ProductTypeName"
                },
                Manufacturer = new Manufacturer
                {
                    Name = "ManufacturerName"
                },
                Pictures = new List<Picture>()
                {
                    new Picture
                    {
                        PictureUrl = "Prod_Three_1"
                    }
                },
                Price = 220.00M,
                Quantity = new Quantity
                {
                    AvailableItems = 1
                }
            };

            db.Products.Add(testProduct);
            await db.SaveChangesAsync();

            var productInDb = db.Products.First();

            var productToAdd = await this.productService
                .GetProductByIdAsync(productInDb.Id);

            await this.productService.CalculateProductPriceAsync(productInDb.Id);

            string userId = db.Users.First().Id;

            var testCart = new ShoppingCart
            {
                UserId = testUser.Id
            };

            db.ShoppingCarts.Add(testCart);
            await db.SaveChangesAsync();

            var shoppingCartItem = new ShoppingCartItem
            {
                Product = productInDb,
                ProductId = productInDb.Id,
                ShoppingCart = testCart,
                ShoppingCartId = testCart.Id,
                Quantity = 1
            };

            db.ShoppingCartItems.Add(shoppingCartItem);
            await db.SaveChangesAsync();

            testCart.ShoppingCartItems.Add(shoppingCartItem);
            db.Update(testCart);
            await db.SaveChangesAsync();


            string productShoppingCartId = productInDb.ShoppingCartId;
            int shoppingCartItemsInDbCount = db.ShoppingCartItems.Count();

            var shoppingCart = db.ShoppingCarts.First();

            int itemsInShoppingCartCount = shoppingCart.ShoppingCartItems.Count();

            bool productRemovedFromCart = await this.shoppingCartService
                .RemoveItemFromCartAsync(userId, shoppingCartItem.Id);

            Assert.True(productRemovedFromCart, errorMessagePrefix + " " + "Returned false");
            Assert.True(productInDb.ShoppingCartId == null, errorMessagePrefix + " " + "CartId of product was not set back to null.");

            int countOfItemsInDbAfterRemoval = db.ShoppingCartItems.Count();

            Assert.True(shoppingCartItemsInDbCount > countOfItemsInDbAfterRemoval, errorMessagePrefix + " " +
                "Num of shopping cart items in db was not decreased after removal.");

            int itemsInCartNumAfterRemoval = shoppingCart.ShoppingCartItems.Count();

            Assert.True(itemsInShoppingCartCount > itemsInCartNumAfterRemoval, errorMessagePrefix + " " +
                "Num of items in shopping cart was not decreased.");
        }

        [Fact]
        public async Task RemoveProductFromCart_WithInvalidUserId_ExpectedToReturnFalse()
        {
            string errorMessagePrefix = "ShoppingCartService RemoveItemFromCartAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            this.productService = new ProductService(db);
            this.shoppingCartService = new ShoppingCartService(db, productService);

            Product testProduct = new Product
            {
                Name = "Product Three",
                Description = "Product Three description",
                Colour = "Green",
                Size = "Medium",
                Category = new Category
                {
                    Name = "CatName"
                },
                ProductType = new ProductType
                {
                    Name = "ProductTypeName"
                },
                Manufacturer = new Manufacturer
                {
                    Name = "ManufacturerName"
                },
                Pictures = new List<Picture>()
                {
                    new Picture
                    {
                        PictureUrl = "Prod_Three_1"
                    }
                },
                Price = 220.00M,
                Quantity = new Quantity
                {
                    AvailableItems = 1
                }
            };

            db.Products.Add(testProduct);
            await db.SaveChangesAsync();

            var productInDb = db.Products.First();

            var productToAdd = await this.productService
                .GetProductByIdAsync(productInDb.Id);

            await this.productService.CalculateProductPriceAsync(productInDb.Id);

            var testCart = new ShoppingCart
            {
                UserId = "1"
            };

            db.ShoppingCarts.Add(testCart);
            await db.SaveChangesAsync();

            var shoppingCartItem = new ShoppingCartItem
            {
                Product = productInDb,
                ProductId = productInDb.Id,
                ShoppingCart = testCart,
                ShoppingCartId = testCart.Id,
                Quantity = 1
            };

            db.ShoppingCartItems.Add(shoppingCartItem);
            await db.SaveChangesAsync();

            testCart.ShoppingCartItems.Add(shoppingCartItem);
            db.Update(testCart);
            await db.SaveChangesAsync();

            bool actualResult = await this.shoppingCartService.RemoveItemFromCartAsync(null, shoppingCartItem.Id);

            Assert.True(actualResult == false, errorMessagePrefix + " " + "Returns true.");
        }
    }
}
