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
                return BadRequest();
            }

            if (this.User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Identity/Account/Login");
            }

            string userId = this.User
                .FindFirst(ClaimTypes.NameIdentifier).Value;

            var product = await this.productService
                .GetProductByIdAsync(productId);

            await this.productService.CalculateProductPriceAsync(product.Id);

            var item = new ShoppingCartItemServiceModel
            {
                Product = product,
                Quantity = 1
            };

            string cartId = await this.shoppingCartService
                .AddItemToCartCartAsync(userId, item);

            return Redirect("/ShoppingCarts/Display");
        }

        [Authorize]
        [HttpGet("/ShoppingCarts/Display")]
        public async Task<IActionResult> Display()
        {
            string userId = this.User
                .FindFirst(ClaimTypes.NameIdentifier).Value;

            var shoppingCartInDb = await this.shoppingCartService
                .GetCartByUserIdAsync(userId);

            if (shoppingCartInDb == null)
            {
                return NotFound();
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
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            bool itemIsRemovedFromCart = await this.shoppingCartService
                .RemoveItemFromCartAsync(userId, itemId);

            return Redirect("/ShoppingCarts/Display");
        }
    }
}
