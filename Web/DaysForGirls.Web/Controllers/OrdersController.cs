using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaysForGirls.Services;
using DaysForGirls.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DaysForGirls.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet("/Orders/Create/{userId}")]
        public async Task<IActionResult> Create(string userId)
        {
            if(userId == null)
            {
                return BadRequest();
            }

            var order = await this.orderService.CreateAsync(userId);

            OrderDisplayViewModel orderToDisplay = new OrderDisplayViewModel
            {
                Id = order.Id,
                IssuedOn = order.IssuedOn.ToString("dddd, dd MMMM yyyy"),
                OrderedProducts = order.OrderedProducts
                    .Select(p => new OrderedProductDisplayViewModel
                    {
                        Id = p.Id,
                        ProductId = p.ProductId,
                        ProductName = p.ProductName,
                        ProductColour = p.ProductColour,
                        ProductSize = p.ProductSize,
                        ProductPrice = p.ProductPrice,
                        ProductPicture = p.ProductPicture,
                        ProductQuantity = p.ProductQuantity
                    }).ToList(),

                TotalPrice = order.TotalPrice,
                UserFullName = order.User.FirstName + " " + order.User.LastName,
                OrderStatus = order.OrderStatus,
                IsDeleted = order.IsDeleted
            };

            return View(orderToDisplay);
        }

        [HttpGet("/Orders/UserAll/{userName}")]
        public async Task<IActionResult> UserAll(string userName)
        {
            var allOrdersOfUser = await this.orderService
                .DisplayAllOrdersOfUser(userName);

            List<OrderDisplayAllViewModel> ordersToReturn = new List<OrderDisplayAllViewModel>();
            int number = 1;
            foreach(var order in allOrdersOfUser)
            {
                var orderToReturn = new OrderDisplayAllViewModel
                {
                    Number = number++,
                    Id = order.Id,
                    IssuedOn = order.IssuedOn.ToString("dddd, dd MMMM yyyy"),
                    ProductsInOrder = order.OrderedProducts
                        .Select(p => new OrderedProductsDisplayAllViewModel
                        {
                            Id = p.Id,
                            ProductName = p.ProductName,
                            ProductPicture = p.ProductPicture,
                            ProductColour = p.ProductColour,
                            ProductSize = p.ProductSize,
                            ProductPrice = p.ProductPrice,
                            ProductQuantity = p.ProductQuantity
                        }).ToList(),
                    TotalPrice = order.TotalPrice,
                    Status = order.OrderStatus
                };

                ordersToReturn.Add(orderToReturn);
            }

            return View(ordersToReturn);
        }

        [HttpGet("/Orders/AllAdmin")]
        public async Task<IActionResult> AllAdmin()
        {
            var allOrdersInDb = await this.orderService
                .DisplayAllOrdersToAdminAsync().ToListAsync();

            var adminOrders = new List<AdminOrdersDisplayAllViewModel>();
            int number = 1;

            foreach (var order in allOrdersInDb)
            {
                var adminOrder = new AdminOrdersDisplayAllViewModel
                {
                    Number = number++,
                    Id = order.Id,
                    IssuedOn = order.IssuedOn.ToString("dd MMMM yyyy"),
                    OrderedProducts = order.OrderedProducts
                        .Select(p => new OrderedProductsDisplayAllViewModel
                        {
                            Id = p.Id,
                            ProductName = p.ProductName,
                            ProductPicture = p.ProductPicture,
                            ProductColour = p.ProductColour,
                            ProductSize = p.ProductSize,
                            ProductPrice = p.ProductPrice,
                            ProductQuantity = p.ProductQuantity
                        })
                        .ToList(),
                    TotalPrice = order.TotalPrice,
                    UserId = order.User.Id,
                    UserFirstName = order.User.FirstName,
                    UserLastName = order.User.LastName,
                    OrderStatus = order.OrderStatus,
                    IsDeleted = order.IsDeleted
                };

                adminOrders.Add(adminOrder);
            }

            return View(adminOrders);
        }
    }
}