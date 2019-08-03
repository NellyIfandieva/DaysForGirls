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
    public class OtherOccasionsController : Controller
    {
        private readonly IProductService productService;

        public OtherOccasionsController(IProductService productService)
        {
            this.productService = productService;
        }

        public IActionResult All()
        {
            var allSpecialOccasionsProducts = this.productService
                .AllOtherProducts()
                .Select(p => new ProductDisplayAllViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Category = p.Category.Name,
                    ProductType = p.ProductType.Name,
                    Price = p.Price,
                    MainPicture = p.MainPicture.PictureUrl,
                    Quantity = p.Quantity.AvailableItems
                })
                .ToListAsync();

            return View(allSpecialOccasionsProducts);
        }
    }
}