using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaysForGirls.Services;
using DaysForGirls.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DaysForGirls.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly IProductService productService;

        public SearchController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpPost("/Search/Display/{criteria}")]
        public async Task<IActionResult> Display(string criteria)
        {
            if(criteria == null)
            {
                return View();
            }
            var searchResults = await this.productService
                .GetAllSearchResultsByCriteria(criteria)
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
                })
                .ToListAsync();

            return View(searchResults);
        }
    }
}