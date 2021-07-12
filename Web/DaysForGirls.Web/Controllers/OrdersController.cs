namespace DaysForGirls.Web.Controllers
{
    using DaysForGirls.Data.Models;
    using InputModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Services;
    using Services.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using ViewModels;

    public class OrdersController : Controller
    {
        private readonly IOrderService orderService;
        private readonly UserManager<DaysForGirlsUser> userManager;

        public OrdersController(IOrderService orderService,
            UserManager<DaysForGirlsUser> userManager)
        {
            this.orderService = orderService;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpGet("/Orders/Create/{userId}")]
        public async Task<IActionResult> Create(string userId)
        {
            if (userId == null)
            {
                return Redirect("/Home/Error");
            }

            var currentUser = await this.userManager.FindByIdAsync(userId);

            if(currentUser == null)
            {
                return Redirect("/Identity/Account/Login");
            }

            var order = await this.orderService
                .CreateAsync(currentUser);

            if (order == null)
            {
                return Redirect("/Home/Error");
            }

            var orderToDisplay = new OrderDisplayViewModel
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
                DeliveryEarliestDate = order.DeliveryEarlistDate,
                DeliveryLatestDate = order.DeliveryLatestDate,
                OrderStatus = order.OrderStatus,
                IsDeleted = order.IsDeleted
            };

            return View(orderToDisplay);
        }

        [HttpGet("/Orders/UserAll/{userName}")]
        public async Task<IActionResult> UserAll(string userName)
        {
            if (userName == null)
            {
                return Redirect("/Identity/Account/Login");
            }

            DaysForGirlsUser requestedUser =
               await this.userManager.FindByNameAsync(userName);

            string currentUserId = this.User
                .FindFirst(ClaimTypes.NameIdentifier).Value;

            if (requestedUser.Id != currentUserId || currentUserId == null)
            {
                return Redirect("/Home/Error");
            }

            var allOrdersOfUser = await this.orderService
                .DisplayAllOrdersOfUserAsync(currentUserId);

            var ordersToReturn = new List<OrderDisplayAllViewModel>();

            int number = 1;

            foreach (var order in allOrdersOfUser)
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

        [Authorize(Roles = "Admin")]
        [HttpGet("/Orders/AllAdmin")]
        public async Task<IActionResult> AllAdmin()
        {
            var allOrdersInDb = await this.orderService
                .DisplayAllOrdersToAdmin();

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

        [Authorize]
        [HttpGet("/Orders/Details/{orderId}")]
        public async Task<IActionResult> Details(string orderId)
        {
            if (orderId == null)
            {
                return Redirect("/Home/Error");
            }

            if (this.User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Identity/Account/Login");
            }

            string currentUserId = this.User
                .FindFirst(ClaimTypes.NameIdentifier).Value;

            bool orderBelongsToUser = await this.orderService
                .CheckIfOrderBelongsToUser(orderId, currentUserId);

            if(orderBelongsToUser == false)
            {
                return Redirect("/Home/Error");
            }

            var orderInDb = await this.orderService
                .GetOrderByIdAsync(orderId);

            if (orderInDb == null)
            {
                return Redirect("/Home/Error");
            }

            var orderToReturn = new OrderDetailsViewModel
            {
                Id = orderInDb.Id,
                DeliveryEarlistDate = orderInDb.DeliveryEarlistDate,
                DeliveryLatestDate = orderInDb.DeliveryLatestDate,
                IssuedOn = orderInDb.IssuedOn.ToString("dddd, dd MMMM yyyy"),
                UserIssuedTo = orderInDb.IssuedTo,
                OrderStatus = orderInDb.OrderStatus,
                TotalPrice = orderInDb.TotalPrice,
                OrderedProducts = orderInDb.OrderedProducts
                    .Select(p => new ProductInOrderViewModel
                    {
                        Id = p.Id,
                        ProductId = p.ProductId,
                        ProductName = p.ProductName,
                        ProductPicture = p.ProductPicture,
                        ProductColour = p.ProductColour,
                        ProductSize = p.ProductSize,
                        ProductPrice = p.ProductPrice,
                        ProductQuantity = p.ProductQuantity
                    })
                    .ToList()
            };

            return View(orderToReturn);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("/Orders/Edit/{orderId}")]
        public async Task<IActionResult> Edit(string orderId)
        {
            if (orderId == null)
            {
                return Redirect("/Home/Error");
            }

            var orderInDb = await this.orderService
                .GetOrderByIdAsync(orderId);

            if (orderInDb == null)
            {
                return Redirect("/Home/Error");
            }

            var orderToEdit = new OrderEditInputModel
            {
                Id = orderInDb.Id,
                IssuedOn = orderInDb.IssuedOn.ToString("dddd, dd MMMM yyyy"),
                UserIssuedTo = orderInDb.IssuedTo,
                DeliveryEarlistDate = orderInDb.DeliveryEarlistDate,
                DeliveryLatestDate = orderInDb.DeliveryLatestDate,
                OrderStatus = orderInDb.OrderStatus,
                TotalPrice = orderInDb.TotalPrice,
                OrderedProductsNum = orderInDb.OrderedProducts.Count()
            };

            return View(orderToEdit);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/Orders/Edit/{orderId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string orderId, OrderEditInputModel model)
        {
            if (orderId == null)
            {
                return Redirect("/Home/Error");
            }

            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            var orderToEdit = new OrderServiceModel
            {
                Id = orderId,
                OrderStatus = model.OrderStatus
            };

            bool orderIsEdited = await this.orderService
                .EditOrderStatusAsync(orderToEdit);

            if(orderIsEdited == false)
            {
                return Redirect("/Home/Error");
            }

            return Redirect("/Orders/Details/" + orderId);
        }
    }
}