namespace DaysForGirls.Services
{
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class OrderService : IOrderService
    {
        private readonly DaysForGirlsDbContext db;
        private readonly IAdminService adminService;

        public OrderService(
            DaysForGirlsDbContext db,
            IAdminService adminService)
        {
            this.db = db;
            this.adminService = adminService;
        }

        public async Task<OrderServiceModel> CreateAsync(DaysForGirlsUser user)
        {
            if(user == null)
            {
                return null;
            }

            string userId = user.Id;

            if(userId == null)
            {
                return null;
            }

            var cart = await this.db.ShoppingCarts
                .Include(c => c.ShoppingCartItems)
                .SingleOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return null;
            }

            var cartItems = await this.db.ShoppingCartItems
                .Include(sCI => sCI.Product)
                .Include(sCI => sCI.Product.Pictures)
                .Where(sCI => sCI.ShoppingCartId == cart.Id).ToListAsync();

            if(cartItems.Count < 1)
            {
                return null;
            }

            var cartItemProductsIds = cartItems.Select(cI => cI.ProductId).ToList();

            var orderProducts = new HashSet<OrderedProduct>();

            foreach (var item in cartItems)
            {
                var product = new OrderedProduct
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

            var order = new Order
            {
                OrderedProducts = orderProducts,
                TotalPrice = orderProducts.Sum(p => p.ProductPrice * p.ProductQuantity),
                User = user,
                UserId = userId,
                OrderStatus = "Ordered"
            };

            this.db.Orders.Add(order);
            int resultOne = await this.db.SaveChangesAsync();

            if (resultOne < 1)
            {
                return null;
            }

            string orderId = order.Id;

            OrderServiceModel orderToReturn = null;

            if (orderId != null)
            {
                user.Orders.Add(order);

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
                            ProductSalePrice = oP.ProductSalePrice,
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

                bool productsInCartAreAddedToOrder = await this.adminService
                    .SetOrderIdToProductsAsync(cartItemProductsIds, orderId);

                if(productsInCartAreAddedToOrder == false)
                {
                    return null;
                }

                this.db.ShoppingCartItems.RemoveRange(cartItems);
                cart.ShoppingCartItems.Clear();
                int resultTwo = await this.db.SaveChangesAsync();
            }

            return orderToReturn;
        }

        public async Task<IEnumerable<OrderServiceModel>> DisplayAllOrdersOfUserAsync(string userId)
        {
            if(userId == null)
            {
                return null;
            }

            var allOrdersOfUser = await this.db.Orders
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
                .ToListAsync();

            return allOrdersOfUser;
        }

        public async Task<IEnumerable<OrderServiceModel>> DisplayAllOrdersToAdmin()
        {
            var allOrdersInDb = await this.db.Orders
                .Include(o => o.OrderedProducts)
                .Include(o => o.User)
                .Select(o => new OrderServiceModel
                {
                    Id = o.Id,
                    IssuedOn = o.IssuedOn,
                    IssuedTo = o.User.FullName,
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
                            ProductSalePrice = p.ProductSalePrice,
                            ProductQuantity = p.ProductQuantity
                        }).ToList(),
                    TotalPrice = o.TotalPrice,
                    User = new DaysForGirlsUserServiceModel
                    {
                        Id = o.User.Id,
                        FirstName = o.User.FirstName,
                        LastName = o.User.LastName
                    },
                    DeliveryEarlistDate = o.DeliveryEarliestDate.ToString("dddd, dd MMMM yyyy"),
                    DeliveryLatestDate = o.DeliveryLatestDate.ToString("dddd, dd MMMM yyyy"),
                    OrderStatus = o.OrderStatus,
                    IsDeleted = o.IsDeleted
                }).ToListAsync();

            return allOrdersInDb;
        }

        public async Task<OrderServiceModel> GetOrderByIdAsync(string orderId)
        {
            if(orderId == null)
            {
                return null;
            }

            var orderInDb = await this.db.Orders
                .Include(o => o.OrderedProducts)
                .Include(o => o.User)
                .SingleOrDefaultAsync(o => o.Id == orderId);

            if (orderInDb == null)
            {
                return null;
            }

            var orderToReturn = new OrderServiceModel
            {
                Id = orderInDb.Id,
                DeliveryEarlistDate = orderInDb.DeliveryEarliestDate.ToString("dddd, dd MMMM yyyy"),
                DeliveryLatestDate = orderInDb.DeliveryLatestDate.ToString("dddd, dd MMMM yyyy"),
                IssuedOn = orderInDb.IssuedOn,
                IssuedTo = orderInDb.User.FullName,
                OrderStatus = orderInDb.OrderStatus,
                TotalPrice = orderInDb.TotalPrice,
                OrderedProducts = orderInDb.OrderedProducts
                    .Select(p => new OrderedProductServiceModel
                    {
                        Id = p.Id,
                        ProductId = p.ProductId,
                        ProductName = p.ProductName,
                        ProductPicture = p.ProductPicture,
                        ProductColour = p.ProductColour,
                        ProductSize = p.ProductSize,
                        ProductPrice = p.ProductPrice,
                        ProductSalePrice = p.ProductSalePrice,
                        ProductQuantity = p.ProductQuantity,

                    })
                    .ToList()
            };

            return orderToReturn;
        }

        public async Task<bool> EditOrderStatusAsync(OrderServiceModel model)
        {
            var orderInDb = await this.db.Orders
                .SingleOrDefaultAsync(o => o.Id == model.Id);

            if (orderInDb == null)
            {
                return false;
            }

            orderInDb.OrderStatus = model.OrderStatus;

            this.db.Update(orderInDb);
            int result = await this.db.SaveChangesAsync();

            bool orderStatusIsEdited = result > 0;

            return orderStatusIsEdited;
        }

        public async Task<bool> CheckIfOrderBelongsToUser(string orderId, string currentUserId)
        {
            if(orderId == null || currentUserId == null)
            {
                return false;
            }

            var requestedOrder = await this.db.Orders
                .Include(o => o.User)
                .SingleOrDefaultAsync(o => o.Id == orderId);

            if(requestedOrder == null)
            {
                return false;
            }

            if(requestedOrder.User.Id != currentUserId)
            {
                return false;
            }

            return true;
        }
    }
}
