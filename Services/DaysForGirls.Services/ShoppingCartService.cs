﻿namespace DaysForGirls.Services
{
    using DaysForGirls.Data;
    using DaysForGirls.Data.Models;
    using DaysForGirls.Services.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class ShoppingCartService : IShoppingCartService
    {
        private readonly DaysForGirlsDbContext db;
        private readonly IProductService productService;

        public ShoppingCartService(
            DaysForGirlsDbContext db,
            IProductService productService)
        {
            this.db = db;
            this.productService = productService;
        }

        public async Task<string> AddItemToCartCartAsync(string userId, ShoppingCartItemServiceModel model)
        {
            if(userId == null || model == null)
            {
                return null;
            }

            var cart = await this.db.ShoppingCarts
                .SingleOrDefaultAsync(u => u.UserId == userId);

            var product = await this.db.Products
                .SingleOrDefaultAsync(p => p.Id == model.Product.Id);

            if (cart == null || product == null)
            {
                return null;
            }

            ShoppingCartItem shoppingCartItem = new ShoppingCartItem
            {
                Product = product,
                ProductId = model.Product.Id,
                ShoppingCartId = cart.Id,
                Quantity = model.Quantity
            };

            this.db.ShoppingCartItems.Add(shoppingCartItem);

            cart.ShoppingCartItems.Add(shoppingCartItem);

            this.db.Update(cart);
            int result = await this.db.SaveChangesAsync();

            string shoppinCartId = cart.Id;

            bool productIsInCart = await this.productService
                .AddProductToShoppingCartAsync(model.Product.Id, shoppinCartId);

            return shoppinCartId;
        }

        public async Task<ShoppingCartServiceModel> GetCartByUserIdAsync(string userId)
        {
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
                .Where(sCI => sCI.ShoppingCartId == cart.Id)
                .Select(sCI => new ShoppingCartItemServiceModel
                {
                    Id = sCI.Id,
                    Product = new ProductAsShoppingCartItem
                    {
                        Id = sCI.Product.Id,
                        Name = sCI.Product.Name,
                        Colour = sCI.Product.Colour,
                        Size = sCI.Product.Size,
                        Price = sCI.Product.Price,
                        SalePrice = sCI.Product.SalePrice,
                        MainPictureUrl = sCI.Product.Pictures.ElementAt(0).PictureUrl
                    },
                    Quantity = sCI.Quantity
                })
                .ToListAsync();

            var cartToReturn = new ShoppingCartServiceModel
            {
                Id = cart.Id,
                ShoppingCartItems = cartItems
            };

            return cartToReturn;
        }

        public async Task<bool> RemoveItemFromCartAsync(string userId, int itemId)
        {
            if(userId == null || itemId <= 0)
            {
                return false;
            }

            var cart = await this.db.ShoppingCarts
                .Include(sC => sC.ShoppingCartItems)
                .SingleOrDefaultAsync(sC => sC.UserId == userId);

            if (cart == null)
            {
                return false;
            }

            var cartItemToDelete = await this.db.ShoppingCartItems
                .SingleOrDefaultAsync(sCI => sCI.Id == itemId);

            if (cartItemToDelete == null)
            {
                return false;
            }

            var productId = cartItemToDelete.ProductId;

            if(productId <= 0)
            {
                return false;
            }

            bool productQuantityIsUpdated = await this.productService
                .RemoveProductFromShoppingCartAsync(productId);

            if (productQuantityIsUpdated == false)
            {
                return false;
            }

            cart.ShoppingCartItems.Remove(cartItemToDelete);

            this.db.Update(cart);

            this.db.ShoppingCartItems.Remove(cartItemToDelete);

            int result = await this.db.SaveChangesAsync();

            bool itemRemovedFromCart = result > 0;

            return itemRemovedFromCart;
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
