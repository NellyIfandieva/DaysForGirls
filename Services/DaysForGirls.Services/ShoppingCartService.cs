namespace DaysForGirls.Services
{
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Services.Models;
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

            var shoppingCartItem = new ShoppingCartItem
            {
                Product = product,
                ProductId = model.Product.Id,
                ShoppingCartId = cart.Id,
                Quantity = model.Quantity
            };

            this.db.ShoppingCartItems.Add(shoppingCartItem);

            cart.ShoppingCartItems.Add(shoppingCartItem);

            this.db.Update(cart);
            int result = await db.SaveChangesAsync();

            string shoppinCartId = cart.Id;

            var productIsInCart = await this.productService
                .AddProductToShoppingCartAsync(model.Product.Id, shoppinCartId);

            if(productIsInCart == null)
            {
                return null;
            }

            return productIsInCart == null ?
                null : 
                shoppinCartId;
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

        public async Task<int?> RemoveItemFromCartAsync(string userId, int itemId)
        {
            if(userId == null || itemId <= 0)
            {
                return null;
            }

            var cart = await this.db.ShoppingCarts
                .Include(sC => sC.ShoppingCartItems)
                .SingleOrDefaultAsync(sC => sC.UserId == userId);

            if (cart == null)
            {
                return null;
            }

            var cartItemToDelete = await this.db.ShoppingCartItems
                .SingleOrDefaultAsync(sCI => sCI.Id == itemId);

            if (cartItemToDelete == null)
            {
                return null;
            }

            var productId = cartItemToDelete.ProductId;

            if(productId <= 0)
            {
                return null;
            }

            var productQuantityIsUpdated = await this.productService
                .RemoveProductFromShoppingCartAsync(productId);

            if (productQuantityIsUpdated == null)
            {
                return null;
            }

            cart.ShoppingCartItems.Remove(cartItemToDelete);

            this.db.Update(cart);

            this.db.ShoppingCartItems.Remove(cartItemToDelete);

            return await db.SaveChangesAsync();
        }
    }
}
