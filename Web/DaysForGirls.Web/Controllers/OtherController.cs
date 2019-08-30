namespace DaysForGirls.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Services;
    using System.Linq;
    using System.Threading.Tasks;
    using ViewModels;

    public class OtherController : Controller
    {
        private readonly IProductService productService;

        public OtherController(IProductService productService)
        {
            this.productService = productService;
        }

        public async Task<IActionResult> All()
        {
            string categoryName = "Other";

            var allOtherProducts = await this.productService
                .GetAllProductsOfCategory(categoryName)
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
                })
                .ToListAsync();

            return View(allOtherProducts);
        }

        public async Task<IActionResult> Dresses()
        {
            string productTypeName = "Dress";
            string categoryName = "Other";

            var allOtherDresses = await this.productService
                .GetAllProductsOfTypeAndCategory(productTypeName, categoryName)
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
                })
                .ToListAsync();

            return View(allOtherDresses);
        }

        public async Task<IActionResult> Suits()
        {
            string productTypeName = "Suit";
            string categoryName = "Other";

            var allOtherSuits = await this.productService
                .GetAllProductsOfTypeAndCategory(productTypeName, categoryName)
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
                })
                .ToListAsync();

            return View(allOtherSuits);
        }

        public async Task<IActionResult> Accessories()
        {
            string productTypeName = "Accessory";
            string categoryName = "Other";

            var allOtherAccessories = await this.productService
                .GetAllProductsOfTypeAndCategory(productTypeName, categoryName)
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
                })
                .ToListAsync();

            return View(allOtherAccessories);
        }
    }
}