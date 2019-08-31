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
    public class OrderServiceTests
    {
        private IOrderService orderService;
        private IAdminService adminService;
        private IPictureService pictureService;
        private ICustomerReviewService customerReviewService;

        private List<Order> GetSampleOrders()
        {
            return new List<Order>()
            {
                new Order
                {
                    IssuedOn = DateTime.UtcNow.AddDays(-5),

                    OrderedProducts = new HashSet<OrderedProduct>(),
                    TotalPrice = 500.00m,
                    User = new DaysForGirlsUser
                    {
                        FirstName = "Johnny",
                        LastName = "Johnson",
                        PasswordHash = "123",
                        PhoneNumber = "0888888889",
                        UserName = "UserTwo",
                        Email = "userTwo@userOne.com",
                        Address = "S"
                    },
                    UserId = "JohnnyJohnson",
                    OrderStatus = "Ordered"
                },
                new Order
                {
                    TotalPrice = 1500.00m,
                    User = new DaysForGirlsUser
                    {
                        FirstName = "Mary",
                        LastName = "Johnson",
                        PasswordHash = "123",
                        PhoneNumber = "0888888888",
                        UserName = "UserTwo",
                        Email = "userOne@userOne.com",
                        Address = "S"
                    },
                    UserId = "MaryJohnson",
                    OrderStatus = "Ordered"
                }
            };
        }

        private async Task SeedSampleOrders(DaysForGirlsDbContext db)
        {
            db.Orders.AddRange(GetSampleOrders());
            await db.SaveChangesAsync();
        }

        [Fact]
        public async Task CreateOrder_WithValidData_ExpectedToReturnAnOrderServiceModel()
        {
            string errorMessagePrefix = "OrderService CreateAsync() method does not work properly.";
            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();

            this.pictureService = new PictureService(db);
            this.customerReviewService = new CustomerReviewService(db);
            this.adminService = new AdminService(db, pictureService, customerReviewService);

            this.orderService = new OrderService(db, adminService);

            var sampleUser = new DaysForGirlsUser
            {
                FirstName = "Johnny",
                LastName = "Johnson",
                PasswordHash = "123",
                PhoneNumber = "0888888889",
                UserName = "UserTwo",
                Email = "userTwo@userOne.com",
                Address = "S"
            };

            db.Users.Add(sampleUser);
            await db.SaveChangesAsync();

            var user = db.Users.First();
            string userId = user.Id;

            var cart = new ShoppingCart
            {
                UserId = userId
            };

            db.ShoppingCarts.Add(cart);
            await db.SaveChangesAsync();

            var sampleShopCartItem = new ShoppingCartItem
            {
                Product = new Product(),
                ShoppingCartId = cart.Id
            };

            db.ShoppingCartItems.Add(sampleShopCartItem);
            await db.SaveChangesAsync();

            cart.ShoppingCartItems.Add(sampleShopCartItem);
            db.Update(cart);
            await db.SaveChangesAsync();

            OrderServiceModel actualResult = await this.orderService
                .CreateAsync(user);

            Assert.True(actualResult != null, errorMessagePrefix + " " + "Does not return an OrderServiceModel");
        }

        [Fact]
        public async Task CreateOrder_WithInValidUser_ExpectedToReturnNull()
        {
            string errorMessagePrefix = "OrderService DisplayAllAdmin() method does not work properly.";
            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();

            this.pictureService = new PictureService(db);
            this.customerReviewService = new CustomerReviewService(db);
            this.adminService = new AdminService(db, pictureService, customerReviewService);

            this.orderService = new OrderService(db, adminService);

            var actualResult = await this.orderService.CreateAsync(null);

            Assert.True(actualResult == null, errorMessagePrefix + " " + "Returns an order.");
        }

        [Fact]
        public async Task DisplayAllToUser_WithValidData_ExpectedToReturnAllOrdersOfTheUser()
        {
            string errorMessagePrefix = "OrderService DisplayAllAdmin() method does not work properly.";
            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();

            var sampleUser = new DaysForGirlsUser
            {
                FirstName = "Johnny",
                LastName = "Johnson",
                PasswordHash = "123",
                PhoneNumber = "0888888889",
                UserName = "UserTwo",
                Email = "userTwo@userOne.com",
                Address = "S"
            };

            db.Users.Add(sampleUser);
            await db.SaveChangesAsync();

            var sampleOrderOne = new Order
            {
                IssuedOn = DateTime.UtcNow.AddDays(-5),

                OrderedProducts = new HashSet<OrderedProduct>(),
                TotalPrice = 500.00m,
                User = new DaysForGirlsUser
                {
                    FirstName = "Johnny",
                    LastName = "Johnson",
                    PasswordHash = "123",
                    PhoneNumber = "0888888889",
                    UserName = "UserTwo",
                    Email = "userTwo@userOne.com",
                    Address = "S"
                },
                UserId = "JohnnyJohnson",
                OrderStatus = "Ordered"
            };

            var sampleOrderTwo = new Order
            {
                TotalPrice = 1500.00m,
                User = new DaysForGirlsUser
                {
                    FirstName = "Mary",
                    LastName = "Johnson",
                    PasswordHash = "123",
                    PhoneNumber = "0888888888",
                    UserName = "UserTwo",
                    Email = "userOne@userOne.com",
                    Address = "S"
                },
                UserId = "MaryJohnson",
                OrderStatus = "Ordered"
            };

            db.Orders.AddRange(sampleOrderOne, sampleOrderTwo);
            await db.SaveChangesAsync();

            this.pictureService = new PictureService(db);
            this.customerReviewService = new CustomerReviewService(db);
            this.adminService = new AdminService(db, pictureService, customerReviewService);

            this.orderService = new OrderService(db, adminService);

            string userId = db.Users.First().Id;

            var expectedResults = db.Orders
                .Where(o => o.UserId == userId)
                .Select(o => new OrderServiceModel
                {
                    Id = o.Id,
                    IssuedOn = o.IssuedOn,
                    OrderedProducts = o.OrderedProducts
                        .Select(p => new OrderedProductServiceModel
                        {
                            Id = p.Id,
                            ProductId = p.ProductId,
                            ProductName = p.ProductName,
                            ProductColour = p.ProductColour,
                            ProductSize = p.ProductSize,
                            ProductPicture = p.ProductPicture,
                            ProductPrice = p.ProductPrice,
                            ProductSalePrice = p.ProductSalePrice,
                            ProductQuantity = p.ProductQuantity
                        }).ToList(),
                    TotalPrice = o.TotalPrice,
                    OrderStatus = o.OrderStatus
                })
                .OrderByDescending(o => o.IssuedOn)
                .ToList();

            var actualResults = await this.orderService.DisplayAllOrdersOfUserAsync(userId);

            Assert.True(expectedResults.Count() == actualResults.Count(), errorMessagePrefix + " " +
                "Does not return correct number of records.");

            for (int i = 0; i < expectedResults.Count(); i++)
            {
                var expectedRecord = expectedResults[i];
                var actualRecord = actualResults[i];

                Assert.True(expectedRecord.IssuedOn == actualRecord.IssuedOn, errorMessagePrefix + " " + "IssuedOn is not returned properly.");
                Assert.True(expectedRecord.OrderedProducts.Count() == actualRecord.OrderedProducts.Count(), errorMessagePrefix + " " + "OrderedProducts Count is not returned properly.");
                Assert.True(expectedRecord.TotalPrice == actualRecord.TotalPrice, errorMessagePrefix + " " + "TotalPrice is not returned properly.");
                Assert.True(expectedRecord.OrderStatus == actualRecord.OrderStatus, errorMessagePrefix + " " + "LogoId is not returned properly.");
            }
        }

        [Fact]
        public async Task DisplayAllToUser_WithNoOrdersForUser_ExpectedToReturnAnEmptyList()
        {
            string errorMessagePrefix = "OrderService DisplayAllAdmin() method does not work properly.";
            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();

            var sampleUser = new DaysForGirlsUser
            {
                FirstName = "Johnny",
                LastName = "Johnson",
                PasswordHash = "123",
                PhoneNumber = "0888888889",
                UserName = "UserTwo",
                Email = "userTwo@userOne.com",
                Address = "S"
            };

            db.Users.Add(sampleUser);
            await db.SaveChangesAsync();

            this.pictureService = new PictureService(db);
            this.customerReviewService = new CustomerReviewService(db);
            this.adminService = new AdminService(db, pictureService, customerReviewService);

            this.orderService = new OrderService(db, adminService);

            string userId = db.Users.First().Id;

            var actualResults = await this.orderService.DisplayAllOrdersOfUserAsync(userId);

            Assert.True(actualResults.Count() == 0, errorMessagePrefix + " " +
                "Does not return correct number of records.");
        }

        [Fact]
        public async Task DisplayAllAdmin_WithDummyData_ExpectedToReturnAllExistingOrders()
        {
            string errorMessagePrefix = "OrderService DisplayAllAdmin() method does not work properly.";
            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();

            await SeedSampleOrders(db);
            this.pictureService = new PictureService(db);
            this.customerReviewService = new CustomerReviewService(db);
            this.adminService = new AdminService(db, pictureService, customerReviewService);

            this.orderService = new OrderService(db, adminService);

            List<OrderServiceModel> expectedResults = db.Orders
                .Select(o => new OrderServiceModel
                {
                    Id = o.Id,
                    IssuedOn = o.IssuedOn,
                    IssuedTo = o.User.FullName,
                    OrderedProducts = new List<OrderedProductServiceModel>(),
                    TotalPrice = o.TotalPrice,
                    User = new DaysForGirlsUserServiceModel
                    {
                        Id = o.User.Id,
                        FirstName = o.User.FirstName,
                        LastName = o.User.LastName
                    },
                    DeliveryEarlistDate = o.DeliveryEarliestDate.ToString(),
                    DeliveryLatestDate = o.DeliveryLatestDate.ToString(),
                    OrderStatus = o.OrderStatus,
                    IsDeleted = o.IsDeleted
                }).ToList();

            List<OrderServiceModel> actualResults = await this.orderService
                .DisplayAllOrdersToAdmin()
                .ToListAsync();


            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedRecord = expectedResults[i];
                var actualRecord = actualResults[i];

                Assert.True(expectedRecord.IssuedOn == actualRecord.IssuedOn, errorMessagePrefix + " " + "IssuedOn is not returned properly.");
                Assert.True(expectedRecord.OrderedProducts.Count() == actualRecord.OrderedProducts.Count(), errorMessagePrefix + " " + "OrderedProducts Count is not returned properly.");
                Assert.True(expectedRecord.TotalPrice == actualRecord.TotalPrice, errorMessagePrefix + " " + "TotalPrice is not returned properly.");
                Assert.True(expectedRecord.OrderStatus == actualRecord.OrderStatus, errorMessagePrefix + " " + "LogoId is not returned properly.");
            }
        }

        [Fact]
        public async Task DisplayAllAdmin_WithNoData_ExpectedToReturnAnEmptyList()
        {
            string errorMessagePrefix = "OrderService DisplayAllAdmin() method does not work properly.";
            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();

            this.pictureService = new PictureService(db);
            this.customerReviewService = new CustomerReviewService(db);
            this.adminService = new AdminService(db, pictureService, customerReviewService);

            this.orderService = new OrderService(db, adminService);

            List<OrderServiceModel> expectedResults = db.Orders
                .Select(o => new OrderServiceModel
                {
                    Id = o.Id,
                    IssuedOn = o.IssuedOn,
                    IssuedTo = o.User.FullName,
                    OrderedProducts = new List<OrderedProductServiceModel>(),
                    TotalPrice = o.TotalPrice,
                    User = new DaysForGirlsUserServiceModel
                    {
                        Id = o.User.Id,
                        FirstName = o.User.FirstName,
                        LastName = o.User.LastName
                    },
                    DeliveryEarlistDate = o.DeliveryEarliestDate.ToString(),
                    DeliveryLatestDate = o.DeliveryLatestDate.ToString(),
                    OrderStatus = o.OrderStatus,
                    IsDeleted = o.IsDeleted
                }).ToList();

            List<OrderServiceModel> actualResults = await this.orderService
                .DisplayAllOrdersToAdmin()
                .ToListAsync();

            Assert.True(expectedResults.Count() == 0 && actualResults.Count() == 0, errorMessagePrefix + " " + "Returned results");
        }

        [Fact]
        public async Task GetById_WithExistingId_ShouldReturnAOrder()
        {
            string errorMessagePrefix = "OrderService GetOrderByIdAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedSampleOrders(db);

            this.pictureService = new PictureService(db);
            this.customerReviewService = new CustomerReviewService(db);
            this.adminService = new AdminService(db, pictureService, customerReviewService);

            this.orderService = new OrderService(db, adminService);

            Order expectedData = db.Orders.First();

            var expectedDataServiceModel = new OrderServiceModel
            {
                Id = expectedData.Id,
                DeliveryEarlistDate = expectedData.DeliveryEarliestDate.ToString("dddd, dd MMMM yyyy"),
                DeliveryLatestDate = expectedData.DeliveryLatestDate.ToString("dddd, dd MMMM yyyy"),
                IssuedOn = expectedData.IssuedOn,
                OrderedProducts = new List<OrderedProductServiceModel>(),
                OrderStatus = expectedData.OrderStatus,
                TotalPrice = expectedData.TotalPrice
            };

            OrderServiceModel actualData = await this.orderService.GetOrderByIdAsync(expectedData.Id);

            Assert.True(expectedDataServiceModel.Id == actualData.Id, errorMessagePrefix + " " + "Id is not returned properly.");
            Assert.True(expectedDataServiceModel.DeliveryEarlistDate == actualData.DeliveryEarlistDate, errorMessagePrefix + " " + "Name is not returned properly.");
            Assert.True(expectedDataServiceModel.DeliveryLatestDate == actualData.DeliveryLatestDate, errorMessagePrefix + " " + "Description is not returned properly");
            Assert.True(expectedDataServiceModel.IssuedOn == actualData.IssuedOn, errorMessagePrefix + " " + "LogoId is not returned properly.");
            Assert.True(expectedDataServiceModel.OrderedProducts.Count() == actualData.OrderedProducts.Count(), errorMessagePrefix + " " + "LogoUrl is not returned properly.");
            Assert.True(expectedDataServiceModel.OrderStatus == actualData.OrderStatus, errorMessagePrefix + " " + "LogoUrl is not returned properly.");
            Assert.True(expectedDataServiceModel.TotalPrice == actualData.TotalPrice, errorMessagePrefix + " " + "LogoUrl is not returned properly.");
        }

        [Fact]
        public async Task GetById_WithNonexistentId_ExpectedToReturnNull()
        {
            string errorMessagePrefix = "OrderService GetOrderByIdAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedSampleOrders(db);

            this.pictureService = new PictureService(db);
            this.customerReviewService = new CustomerReviewService(db);
            this.adminService = new AdminService(db, pictureService, customerReviewService);

            this.orderService = new OrderService(db, adminService);

            var actualResult = await this.orderService.GetOrderByIdAsync("8");

            Assert.True(actualResult == null, errorMessagePrefix + " " + "Returns a nonexisting order.");
        }

        [Fact]
        public async Task CheckIfOrderBelongsToUser_WithAllValidData_ExpectedToReturnTrue()
        {
            string errorMessagePrefix = "OrderService GetOrderByIdAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedSampleOrders(db);

            this.pictureService = new PictureService(db);
            this.customerReviewService = new CustomerReviewService(db);
            this.adminService = new AdminService(db, pictureService, customerReviewService);

            this.orderService = new OrderService(db, adminService);

            var testUser = new DaysForGirlsUser
            {
                FirstName = "Johnny",
                LastName = "Johnson",
                PasswordHash = "123",
                PhoneNumber = "0888888889",
                UserName = "UserTwo",
                Email = "userTwo@userOne.com",
                Address = "S"
            };

            db.Users.Add(testUser);
            await db.SaveChangesAsync();

            var userId = db.Users.First().Id;

            Order testOrder = new Order
            {
                IssuedOn = DateTime.UtcNow.AddDays(-5),
                OrderedProducts = new HashSet<OrderedProduct>(),
                OrderStatus = "Ordered",
                User = testUser,
                UserId = userId
            };

            db.Orders.Add(testOrder);
            await db.SaveChangesAsync();

            bool actualResult = await this.orderService.CheckIfOrderBelongsToUser(testOrder.Id, userId);

            Assert.True(actualResult, errorMessagePrefix + " " + "Returns false");
        }

        [Fact]
        public async Task CheckIfOrderBelongsToUser_WithInvalidOrderId_ExpectedToReturnFalse()
        {
            string errorMessagePrefix = "OrderService GetOrderByIdAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedSampleOrders(db);

            this.pictureService = new PictureService(db);
            this.customerReviewService = new CustomerReviewService(db);
            this.adminService = new AdminService(db, pictureService, customerReviewService);

            this.orderService = new OrderService(db, adminService);

            var testUser = new DaysForGirlsUser
            {
                FirstName = "Johnny",
                LastName = "Johnson",
                PasswordHash = "123",
                PhoneNumber = "0888888889",
                UserName = "UserTwo",
                Email = "userTwo@userOne.com",
                Address = "S"
            };

            db.Users.Add(testUser);
            await db.SaveChangesAsync();

            var userId = db.Users.First().Id;

            bool actualResult = await this.orderService.CheckIfOrderBelongsToUser("order", userId);

            Assert.True(actualResult == false, errorMessagePrefix + " " + "Returns true.");
        }

        [Fact]
        public async Task CheckIfOrderBelongsToUser_WithInvalidUserId_ExpectedToReturnFalse()
        {
            string errorMessagePrefix = "OrderService GetOrderByIdAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedSampleOrders(db);

            this.pictureService = new PictureService(db);
            this.customerReviewService = new CustomerReviewService(db);
            this.adminService = new AdminService(db, pictureService, customerReviewService);

            this.orderService = new OrderService(db, adminService);

            var testUser = new DaysForGirlsUser
            {
                FirstName = "Johnny",
                LastName = "Johnson",
                PasswordHash = "123",
                PhoneNumber = "0888888889",
                UserName = "UserTwo",
                Email = "userTwo@userOne.com",
                Address = "S"
            };

            db.Users.Add(testUser);
            await db.SaveChangesAsync();

            var userId = db.Users.First().Id;

            var testOrder = new Order
            {
                IssuedOn = DateTime.UtcNow.AddDays(-5),
                OrderedProducts = new HashSet<OrderedProduct>(),
                OrderStatus = "Ordered",
                User = testUser,
                UserId = userId
            };

            db.Orders.Add(testOrder);
            await db.SaveChangesAsync();

            bool actualResult = await this.orderService.CheckIfOrderBelongsToUser(testOrder.Id, "johnny");

            Assert.True(actualResult == false, errorMessagePrefix + " " + "Returns true.");
        }
    }
}
