﻿namespace DaysForGirls.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using System.Linq;
    using System.Threading.Tasks;
    using ViewModels;

    public class SearchController : Controller
    {
        private readonly IProductService productService;

        public SearchController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet("/Search/Display/{criteria}")]
        public async Task<IActionResult> Display(string criteria)
        {
            if(criteria == null)
            {
                return Redirect("/Home/Error");
            }

            var productsFromDb = await this.productService
                .GetAllSearchResultsByCriteria(criteria);

            var searchResults = productsFromDb
                .Select(p => new ProductSearchResultViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Category = p.Category.Name,
                    ProductType = p.ProductType.Name,
                    Manufacturer = p.Manufacturer.Name,
                    Colour = p.Colour,
                    Size = p.Size,
                    AvailableItems = p.Quantity.AvailableItems,
                    Picture = p.Pictures.ElementAt(0).PictureUrl,
                    Price = p.Price,
                    SaleId = p.SaleId,
                    ShoppingCartId = p.ShoppingCartId,
                    OrderId = p.OrderId
                });

            return View(searchResults);
        }
    }
}