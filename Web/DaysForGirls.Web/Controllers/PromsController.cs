namespace DaysForGirls.Web.Controllers
{
    using Services;
    using ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    public class PromsController : Controller
    {
        private readonly IProductService productService;

        public PromsController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet("/Proms/All")]
        public async Task<IActionResult> All()
        {
            string categoryName = "Prom";

            var allPromProducts = await this.productService
                .GetAllProductsOfCategory(categoryName)
                .Select(p => new DisplayAllOfCategoryViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Picture = p.Picture.PictureUrl,
                    Price = p.Price.ToString("f2"),
                    AvailableItems = p.AvailableItems,
                    IsInSale = p.IsInSale,
                    SaleId = p.SaleId,
                    ShoppingCartId = p.ShoppingCartId
                })
                .ToListAsync();

            return View(allPromProducts);
        }

        [HttpGet("/Proms/Dresses")]
        public async Task<IActionResult> Dresses()
        {
            string productTypeName = "Dress";
            string categoryName = "Prom";

            var allPromDresses = await this.productService
                .GetAllProductsOfTypeAndCategory(productTypeName, categoryName)
                .Select(d => new DisplayAllOfCategoryAndTypeViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    Picture = d.Picture.PictureUrl,
                    Price = d.Price.ToString("f2"),
                    AvailableItems = d.AvailableItems,
                    SaleId = d.SaleId,
                    ShoppingCartId = d.ShoppingCartId
                })
                .ToListAsync();

            return View(allPromDresses);
        }

        [HttpGet("/Proms/Suits")]
        public async Task<IActionResult> Suits()
        {
            string productTypeName = "Suit";
            string categoryName = "Prom";

            var allPromSuits = await this.productService
                .GetAllProductsOfTypeAndCategory(productTypeName, categoryName)
                .Select(s => new DisplayAllOfCategoryAndTypeViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Picture = s.Picture.PictureUrl,
                    Price = s.Price.ToString("f2"),
                    AvailableItems = s.AvailableItems,
                    SaleId = s.SaleId,
                    ShoppingCartId = s.ShoppingCartId
                })
                .ToListAsync();

            return View(allPromSuits);
        }

        [HttpGet("/Proms/Accessories")]
        public async Task<IActionResult> Accessories()
        {
            string productTypeName = "Accessory";
            string categoryName = "Prom";

            var allPromAccessories = await this.productService
                .GetAllProductsOfTypeAndCategory(productTypeName, categoryName)
                .Select(a => new DisplayAllOfCategoryAndTypeViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Picture = a.Picture.PictureUrl,
                    Price = a.Price.ToString("f2"),
                    AvailableItems = a.AvailableItems,
                    SaleId = a.SaleId,
                    ShoppingCartId = a.ShoppingCartId
                })
                .ToListAsync();

            return View(allPromAccessories);
        }
    }
}