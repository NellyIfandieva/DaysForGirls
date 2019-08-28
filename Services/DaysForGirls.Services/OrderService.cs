namespace DaysForGirls.Services
{
    using DaysForGirls.Data;
    using DaysForGirls.Data.Models;
    using DaysForGirls.Services.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class OrderService : IOrderService
    {
        private readonly DaysForGirlsDbContext db;
        private readonly UserManager<DaysForGirlsUser> userManager;
        private readonly IAdminService adminService;

        public OrderService(
            DaysForGirlsDbContext db,
            UserManager<DaysForGirlsUser> userManager,
            IAdminService adminService)
        {
            this.db = db;
            this.userManager = userManager;
            this.adminService = adminService;
        }

        public async Task<OrderServiceModel> CreateAsync(string userId)
        {
            DaysForGirlsUser orderUser =
                await this.userManager.FindByIdAsync(userId);

            var cart = await this.db.ShoppingCarts
                .Include(c => c.ShoppingCartItems)
                .SingleOrDefaultAsync(c => c.UserId == userId);

            var cartItems = await this.db.ShoppingCartItems
                .Include(sCI => sCI.Product)
                .Include(sCI => sCI.Product.Pictures)
                .Where(sCI => sCI.ShoppingCartId == cart.Id).ToListAsync();

            var cartItemProductsIds = cartItems.Select(cI => cI.ProductId).ToList();

            HashSet<OrderedProduct> orderProducts = new HashSet<OrderedProduct>();

            foreach(var item in cartItems)
            {
                OrderedProduct product = new OrderedProduct
                {
                    ProductId = item.Product.Id,
                    ProductName = item.Product.Name,
                    ProductColour = item.Product.Colour,
                    ProductSize = item.Product.Size,
                    ProductPrice = item.Product.Price,
                    ProductPicture = item.Product.Pictures.ElementAt(0).PictureUrl,
                    ProductQuantity = item.Quantity
                };

                orderProducts.Add(product);
            }

            this.db.OrderedProducts.AddRange(orderProducts);

            Order order = new Order
            {
                OrderedProducts = orderProducts,
                TotalPrice = orderProducts.Sum(p => p.ProductPrice),
                User = orderUser,
                UserId = userId,
                OrderStatus = "Ordered"
            };

            this.db.Orders.Add(order);
            int resultOne = await this.db.SaveChangesAsync();

            string orderId = order.Id;

            OrderServiceModel orderToReturn = null;

            if(orderId != null)
            {
                orderUser.Orders.Add(order);

                orderToReturn = new OrderServiceModel
                {
                    Id = order.Id,
                    IssuedOn = order.IssuedOn,
                    OrderedProducts = order.OrderedProducts
                        .Select(oP => new OrderedProductServiceModel
                        {
                            Id = oP.Id,
                            ProductId = oP.ProductId,
                            ProductName = oP.ProductName,
                            ProductColour = oP.ProductColour,
                            ProductSize = oP.ProductSize,
                            ProductPicture = oP.ProductPicture,
                            ProductPrice = oP.ProductPrice,
                            ProductQuantity = oP.ProductQuantity
                        }).ToList(),
                    TotalPrice = order.TotalPrice,
                    User = new DaysForGirlsUserServiceModel
                    {
                        Id = order.User.Id,
                        FirstName = order.User.FirstName,
                        LastName = order.User.LastName
                    },
                    DeliveryEarlistDate = order.DeliveryEarliestDate.ToString("dddd, dd MMMM yyyy"),
                    DeliveryLatestDate = order.DeliveryLatestDate.ToString("dddd, dd MMMM yyyy"),
                    OrderStatus = order.OrderStatus,
                    IsDeleted = order.IsDeleted
                };

                bool productInCartAreAddedToOrder = await this.adminService
                    .SetOrderIdToProductsAsync(cartItemProductsIds, orderId);

                this.db.ShoppingCartItems.RemoveRange(cartItems);
                cart.ShoppingCartItems.Clear();
                int resultTwo = await this.db.SaveChangesAsync();
            }

            return orderToReturn;
        }

        public async Task<List<OrderServiceModel>> DisplayAllOrdersOfUser(string userName)
        {
            DaysForGirlsUser currentUser = await this.userManager.FindByNameAsync(userName);

            var allOrdersOfUser = await this.db.Orders
                .Where(o => o.UserId == currentUser.Id)
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
                            ProductQuantity = p.ProductQuantity
                        }).ToList(),
                    TotalPrice = o.TotalPrice,
                    OrderStatus = o.OrderStatus
                })
                .OrderByDescending(o => o.IssuedOn)
                .ToListAsync();

            return allOrdersOfUser;
        }

        public IQueryable<OrderServiceModel> DisplayAllOrdersToAdminAsync()
        {
            var allOrdersInDb = this.db.Orders
                .Include(o => o.OrderedProducts)
                .Include(o => o.User)
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
                            ProductPicture = p.ProductPicture,
                            ProductColour = p.ProductColour,
                            ProductSize = p.ProductSize,
                            ProductPrice = p.ProductPrice,
                            ProductQuantity = p.ProductQuantity
                        }).ToList(),
                    TotalPrice = o.TotalPrice,
                    User = new DaysForGirlsUserServiceModel
                    {
                        Id = o.User.Id,
                        FirstName = o.User.FirstName,
                        LastName = o.User.LastName
                    },
                    OrderStatus = o.OrderStatus,
                    IsDeleted = o.IsDeleted
                });

            return allOrdersInDb;
        }
    }
}
