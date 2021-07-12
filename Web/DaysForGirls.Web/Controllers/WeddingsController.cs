namespace DaysForGirls.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using System.Linq;
    using System.Threading.Tasks;
    using ViewModels;

    public class WeddingsController : Controller
    {
        private readonly IProductService productService;

        public WeddingsController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet("/Weddings/All")]
        public async Task<IActionResult> All()
        {
            string categoryName = "Wedding";

            var allWeddingProducts = await this.productService
                .GetAllProductsOfCategory(categoryName);

            var viewModels = allWeddingProducts
                .Select(p => new DisplayAllOfCategoryViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Picture = p.Picture.PictureUrl,
                    Price = p.Price,
                    SalePrice = p.SalePrice,
                    AvailableItems = p.AvailableItems,
                    IsInSale = p.IsInSale,
                    SaleId = p.SaleId,
                    ShoppingCartId = p.ShoppingCartId,
                    OrderId = p.OrderId
                });

            return View(viewModels);
        }


        [HttpGet("/Weddings/Dresses")]
        public async Task<IActionResult> Dresses()
        {
            string productTypeName = "Dress";
            string categoryName = "Wedding";

            var allWeddingDresses = await this.productService
                .GetAllProductsOfTypeAndCategory(productTypeName, categoryName);

            var viewModels = allWeddingDresses
                .Select(d => new DisplayAllOfCategoryAndTypeViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    Picture = d.Picture.PictureUrl,
                    Price = d.Price,
                    SalePrice = d.SalePrice,
                    AvailableItems = d.AvailableItems,
                    SaleId = d.SaleId,
                    ShoppingCartId = d.ShoppingCartId,
                    OrderId = d.OrderId
                });

            return View(viewModels);
        }

        [HttpGet("/Weddings/Suits")]
        public async Task<IActionResult> Suits()
        {
            string productTypeName = "Suit";
            string categoryName = "Wedding";

            var allWeddingSuits = await this.productService
                .GetAllProductsOfTypeAndCategory(productTypeName, categoryName);

            var viewModels = allWeddingSuits
                .Select(s => new DisplayAllOfCategoryAndTypeViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Picture = s.Picture.PictureUrl,
                    Price = s.Price,
                    SalePrice = s.SalePrice,
                    AvailableItems = s.AvailableItems,
                    SaleId = s.SaleId,
                    ShoppingCartId = s.ShoppingCartId,
                    OrderId = s.OrderId
                });

            return View(viewModels);
        }

        [HttpGet("/Weddings/Accessories")]
        public async Task<IActionResult> Accessories()
        {
            string productTypeName = "Accessory";
            string categoryName = "Wedding";

            var allWeddingAccessories = await this.productService
                .GetAllProductsOfTypeAndCategory(productTypeName, categoryName);

            var viewModels = allWeddingAccessories
                .Select(a => new DisplayAllOfCategoryAndTypeViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Picture = a.Picture.PictureUrl,
                    Price = a.Price,
                    SalePrice = a.SalePrice,
                    AvailableItems = a.AvailableItems,
                    SaleId = a.SaleId,
                    ShoppingCartId = a.ShoppingCartId,
                    OrderId = a.OrderId
                });

            return View(viewModels);
        }
    }
}