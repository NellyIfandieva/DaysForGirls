using DaysForGirls.Data;
using DaysForGirls.Data.Models;
using DaysForGirls.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DaysForGirls.Services
{
    public class ShoppingCartServiceModel
    {
        private readonly DaysForGirlsDbContext db;

        public ShoppingCartServiceModel(DaysForGirlsDbContext db)
        {
            this.db = db;
        }

        public string Id { get; set; }

        public ICollection<ShoppingCartItemServiceModel> ShoppingCartItems { get; set; }
    }

    public class ShoppingCartService : IShoppingCartService
    {
        private readonly DaysForGirlsDbContext db;
        private readonly IServiceProvider services;

        public ShoppingCartService(DaysForGirlsDbContext db,
            IServiceProvider services)
        {
            this.db = db;
            this.services = services;
        }

        private ShoppingCart GetShoppingCart(IServiceProvider service)
        {
            ISession session = this.services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            var context = this.services.GetService<DaysForGirlsDbContext>();
            string cartId = session.GetString("Id") ?? Guid.NewGuid().ToString();

            session.SetString("Id", cartId);

            ShoppingCartServiceModel shoppingCartServiceModel = 
                new ShoppingCartServiceModel(context) { Id = cartId };

            ShoppingCart shoppingCart = new ShoppingCart
            {
                Id = shoppingCartServiceModel.Id
            };

            return shoppingCart;
        }

        public async Task<bool> Add(ShoppingCartItemServiceModel model)
        {
            ShoppingCart cart = this.GetShoppingCart(this.services);
            bool isAdded = await this.AddToCart(cart, model);

            return isAdded;
        }

        private async Task<bool> AddToCart(
            ShoppingCart cart,
            ShoppingCartItemServiceModel item)
        {
            var shoppingCartItem = this.db.ShoppingCartItems
                .SingleOrDefault(s => s.Id == item.Product.Id
                && s.ShoppingCartId == cart.Id);

            if(shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ProductId = item.Product.Id,
                    Quantity = item.Quantity,
                    ShoppingCart = cart
                };

                this.db.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Quantity++;
            }

            Product product = this.db.Products.SingleOrDefault(p => p.Id == shoppingCartItem.ProductId);
            product.Quantity.AvailableItems--;
            this.db.Update(product);

            int result = await this.db.SaveChangesAsync();

            return result == 1;
        }
    }
}
