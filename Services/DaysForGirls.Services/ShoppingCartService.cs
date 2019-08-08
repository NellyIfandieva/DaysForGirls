using DaysForGirls.Data;
using DaysForGirls.Data.Models;
using DaysForGirls.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DaysForGirls.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly DaysForGirlsDbContext db;
        private readonly UserManager<DaysForGirlsUser> userManager;

        public ShoppingCartService(
            DaysForGirlsDbContext db,
            UserManager<DaysForGirlsUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<bool> CreateCart(string userId, ShoppingCartItemServiceModel model)
        {
            var cart = this.db.ShoppingCarts
                .SingleOrDefault(u => u.UserId == userId);

            if(cart == null)
            {
                DaysForGirlsUser currentUser =
                await this.userManager.FindByIdAsync(userId);

                cart = new ShoppingCart
                {
                    User = currentUser,
                    ShoppingCartItems = new List<ShoppingCartItem>()
                };
            }

            ShoppingCartItem shoppingCartItem = new ShoppingCartItem
            {
                ProductId = model.Product.Id,
                Quantity = 1
            };

            cart.ShoppingCartItems.Add(shoppingCartItem);

            if(cart.ShoppingCartItems.Count() <= 1)
            {
                this.db.ShoppingCarts.Add(cart);
            }
            else
            {
                this.db.Update(cart);
            }

            int result = await this.db.SaveChangesAsync();

            bool itemIsAddedToCart = result > 0;

            return itemIsAddedToCart;
        }

        public Task<bool> CreateItem(ShoppingCartItemServiceModel model)
        {
            throw new NotImplementedException();
        }


        //    private readonly DaysForGirlsDbContext db;

        //    public ShoppingCartService(DaysForGirlsDbContext db)
        //    {
        //        this.db = db;
        //    }

        //    public string Id { get; set; }

        //    public ICollection<ShoppingCartItemServiceModel> ShoppingCartItems { get; set; }
        //}

        //public class ShoppingCartService : IShoppingCartService
        //{
        //    private readonly DaysForGirlsDbContext db;
        //    private readonly IServiceProvider services;

        //    public ShoppingCartService(DaysForGirlsDbContext db,
        //        IServiceProvider services)
        //    {
        //        this.db = db;
        //        this.services = services;
        //    }

        //    public async Task<bool> CreateItem(ShoppingCartItemServiceModel model)
        //    {
        //        ShoppingCartItem cartItem = new ShoppingCartItem
        //        {
        //            ProductId = model.Product.Id,
        //            Quantity = model.Quantity
        //        };
        //    }

        //    public Task<bool> CreateCart(ShoppingCartServiceModel model)
        //    {
        //        var currentUse

        //        ShoppingCart cart = new ShoppingCart
        //        {

        //        }
        //    }

        //private ShoppingCart GetShoppingCart(IServiceProvider service)
        //{
        //    ISession session = this.services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

        //    var context = this.services.GetService<DaysForGirlsDbContext>();
        //    string cartId = session.GetString("Id") ?? Guid.NewGuid().ToString();

        //    session.SetString("Id", cartId);

        //    ShoppingCartServiceModel shoppingCartServiceModel = 
        //        new ShoppingCartServiceModel(context) { Id = cartId };

        //    ShoppingCart shoppingCart = new ShoppingCart
        //    {
        //        Id = shoppingCartServiceModel.Id
        //    };

        //    return shoppingCart;
        //}

        //public async Task<bool> Add(ShoppingCartItemServiceModel model)
        //{
        //    ShoppingCart cart = this.GetShoppingCart(this.services);
        //    bool isAdded = await this.AddToCart(cart, model);

        //    return isAdded;
        //}

        //private async Task<bool> AddToCart(
        //    ShoppingCart cart,
        //    ShoppingCartItemServiceModel item)
        //{
        //    var shoppingCartItem = this.db.ShoppingCartItems
        //        .SingleOrDefault(s => s.Id == item.Product.Id
        //        && s.ShoppingCartId == cart.Id);

        //    if(shoppingCartItem == null)
        //    {
        //        shoppingCartItem = new ShoppingCartItem
        //        {
        //            ProductId = item.Product.Id,
        //            Quantity = item.Quantity,
        //            ShoppingCart = cart
        //        };

        //        this.db.ShoppingCartItems.Add(shoppingCartItem);
        //    }
        //    else
        //    {
        //        shoppingCartItem.Quantity++;
        //    }

        //    Product product = this.db.Products.SingleOrDefault(p => p.Id == shoppingCartItem.ProductId);
        //    product.Quantity.AvailableItems--;
        //    this.db.Update(product);

        //    int result = await this.db.SaveChangesAsync();

        //    return result == 1;
        //}
    }
}
