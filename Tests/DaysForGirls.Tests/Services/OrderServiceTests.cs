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
                    TotalPrice = 500.00m,
                    UserId = "Nelly",
                    OrderStatus = "Ordered"
                },
                new Order
                {
                    TotalPrice = 1500.00m,
                    UserId = "Ivan",
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
        public async Task DisplayAllAdmin_WithDummyData_ExpectedToReturnAllExistingOrders()
        {
            string errorMessagePrefix = "OrderService DisplayAllAdmin() method does not work properly.";
            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();

            await SeedSampleOrders(db);
            var userManager = UserManagerMOQ.TestUserManager<DaysForGirlsUser>();

            this.pictureService = new PictureService(db);
            this.customerReviewService = new CustomerReviewService(db);
            this.adminService = new AdminService(db, pictureService, customerReviewService);

            this.orderService = new OrderService(db, userManager, adminService);

            List<OrderServiceModel> expectedResults = GetSampleOrders()
                .Select(o => new OrderServiceModel
                {
                    Id = o.Id,
                    IssuedOn = o.IssuedOn,
                    OrderedProducts = new List<OrderedProductServiceModel>(),
                    TotalPrice = o.TotalPrice,
                    User = new DaysForGirlsUserServiceModel
                    {
                        Id = o.User.Id
                    },
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
                //Assert.True(expectedRecord.Logo.LogoUrl == actualRecord.Logo.LogoUrl, errorMessagePrefix + " " + "LogoUrl is not returned properly.");
            }
        }

        [Fact]
        public async Task GetById_WithExistingId_ShouldReturnAOrder()
        {
            string errorMessagePrefix = "OrderService GetOrderByIdAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            await SeedSampleOrders(db);

            var userManager = UserManagerMOQ.TestUserManager<DaysForGirlsUser>();

            this.pictureService = new PictureService(db);
            this.customerReviewService = new CustomerReviewService(db);
            this.adminService = new AdminService(db, pictureService, customerReviewService);

            this.orderService = new OrderService(db, userManager, adminService);

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
    }
}
