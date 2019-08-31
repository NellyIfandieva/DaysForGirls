namespace DaysForGirls.Web.Controllers
{
    using DaysForGirls.Data;
    using DaysForGirls.Services;
    using DaysForGirls.Services.Models;
    using DaysForGirls.Web.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class ShoppingCartsController : Controller
    {
        private readonly DaysForGirlsDbContext db;
        private readonly IShoppingCartService shoppingCartService;
        private readonly IAdminService adminService;
        private readonly IProductService productService;

        public ShoppingCartsController(
            DaysForGirlsDbContext db,
            IShoppingCartService shoppingCartService,
            IAdminService adminService,
            IProductService productService)
        {
            this.db = db;
            this.shoppingCartService = shoppingCartService;
            this.adminService = adminService;
            this.productService = productService;
        }

        [Authorize]
        [HttpGet("/ShoppingCarts/AddProduct/{productId}")]
        public async Task<IActionResult> AddProduct(int productId)
        {
            if (productId <= 0)
            {
                return Redirect("/Home/Error");
            }

            if (this.User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Identity/Account/Login");
            }

            string userId = this.User
                .FindFirst(ClaimTypes.NameIdentifier).Value;

            if (userId == null)
            {
                return Redirect("/Home/Error");
            }

            var product = await this.productService
                .GetProductByIdAsync(productId);

            if (product == null)
            {
                return Redirect("/Home/Error");
            }

            decimal productPrice = await this.productService.CalculateProductPriceAsync(product.Id);

            if(productPrice == 0.00m)
            {
                return Redirect("/Home/Error");
            }

            var item = new ShoppingCartItemServiceModel
            {
                Product = product,
                Quantity = 1
            };

            string cartId = await this.shoppingCartService
                .AddItemToCartCartAsync(userId, item);

            if(cartId == null)
            {
                return Redirect("/Home/Error");
            }

            return Redirect("/ShoppingCarts/Display");
        }

        [Authorize]
        [HttpGet("/ShoppingCarts/Display")]
        public async Task<IActionResult> Display()
        {
            string userId = this.User
                .FindFirst(ClaimTypes.NameIdentifier).Value;

            if (userId == null)
            {
                return Redirect("/Identity/Account/Login");
            }

            var shoppingCartInDb = await this.shoppingCartService
                .GetCartByUserIdAsync(userId);

            if (shoppingCartInDb == null)
            {
                return Redirect("/Identity/Account/Login");
            }

            var cartToReturn = new ShoppingCartDisplayViewModel
            {
                Id = shoppingCartInDb.Id,
                Items = shoppingCartInDb.ShoppingCartItems
                    .Select(i => new ShoppingCartItemViewModel
                    {
                        Id = i.Id,
                        Product = new ProductAsCartItemViewModel
                        {
                            Id = i.Product.Id,
                            Name = i.Product.Name,
                            Picture = i.Product.MainPictureUrl,
                            Colour = i.Product.Colour,
                            Size = i.Product.Size,
                            Price = i.Product.Price
                        },
                        Quantity = i.Quantity
                    })
                    .ToList(),
                Total = shoppingCartInDb.ShoppingCartItems
                .Sum(p => p.Product.Price),
                UserId = userId
            };

            return View(cartToReturn);
        }

        [Authorize]
        [HttpGet("/ShoppingCarts/RemoveItem/{itemId}")]
        public async Task<IActionResult> RemoveItem(int itemId)
        {
            if(itemId <= 0)
            {
                return Redirect("/Home/Error");
            }

            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if(userId == null)
            {
                return Redirect("/Identity/Account/Login");
            }

            bool itemIsRemovedFromCart = await this.shoppingCartService
                .RemoveItemFromCartAsync(userId, itemId);

            if(itemIsRemovedFromCart == false)
            {
                return Redirect("/Home/Error");
            }

            return Redirect("/ShoppingCarts/Display");
        }
    }
}
