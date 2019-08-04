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
    public class OtherController : Controller
    {
        private readonly IProductService productService;

        public OtherController(IProductService productService)
        {
            this.productService = productService;
        }

        public async Task<IActionResult> All()
        {
            var allSpecialOccasionsProducts = await this.productService
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

        public async Task<IActionResult> Dresses()
        {
            var allOtherDresses = await this.productService
                .AllOtherDresses()
                .Select(d => new ProductDisplayAllViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    Category = d.Category.Name,
                    ProductType = d.ProductType.Name,
                    Price = d.Price,
                    MainPicture = d.MainPicture.PictureUrl,
                    Quantity = d.Quantity.AvailableItems
                })
                .ToListAsync();

            return View(allOtherDresses);
        }

        public async Task<IActionResult> Suits()
        {
            var allOtherSuits = await this.productService
                .AllOtherSuits()
                .Select(s => new ProductDisplayAllViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Category = s.Category.Name,
                    ProductType = s.ProductType.Name,
                    Price = s.Price,
                    MainPicture = s.MainPicture.PictureUrl,
                    Quantity = s.Quantity.AvailableItems
                })
                .ToListAsync();

            return View(allOtherSuits);
        }

        public async Task<IActionResult> Accessories()
        {
            var allOtherAccessories = await this.productService
                .AllOtherAccessories()
                .Select(a => new ProductDisplayAllViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Category = a.Category.Name,
                    ProductType = a.ProductType.Name,
                    Price = a.Price,
                    MainPicture = a.MainPicture.PictureUrl,
                    Quantity = a.Quantity.AvailableItems
                })
                .ToListAsync();

            return View(allOtherAccessories);
        }
    }
}