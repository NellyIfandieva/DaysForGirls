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
    public class WeddingsController : Controller
    {
        private readonly IProductService productService;

        public WeddingsController(
            IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet("/Weddings/All")]
        public async Task<IActionResult> All()
        {
            var allWeddingProducts = await this.productService
                .AllWeddingProducts()
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

            return View(allWeddingProducts);
        }

        [HttpGet("/Weddings/Dresses")]
        public async Task<IActionResult> Dresses()
        {
            var allWeddingDresses = await this.productService
                .AllWeddingDresses()
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

            return View(allWeddingDresses);
        }

        [HttpGet("/Weddings/Suits")]
        public async Task<IActionResult> Suits()
        {
            var allWeddingSuits = await this.productService
                .AllWeddingSuits()
                .Select(suit => new ProductDisplayAllViewModel
                {
                    Id = suit.Id,
                    Name = suit.Name,
                    Category = suit.Category.Name,
                    ProductType = suit.ProductType.Name,
                    Price = suit.Price,
                    MainPicture = suit.MainPicture.PictureUrl,
                    Quantity = suit.Quantity.AvailableItems
                })
                .ToListAsync();

            return View(allWeddingSuits);
        }

        [HttpGet("/Weddings/Accessories")]
        public async Task<IActionResult> Accessories()
        {
            var allWeddingAccessories = await this.productService
                .AllWeddingAccessories()
                .ToListAsync();

            List<ProductDisplayAllViewModel> allAccessoriesToDisplay =
                new List<ProductDisplayAllViewModel>();

            foreach(var accessory in allWeddingAccessories)
            {
                ProductDisplayAllViewModel a = new ProductDisplayAllViewModel
                {
                    Id = accessory.Id,
                    Name = accessory.Name,
                    Price = accessory.Price,
                    Category = accessory.Category.Name,
                    MainPicture = accessory.MainPicture.PictureUrl
                };

                allAccessoriesToDisplay.Add(a);
            }

            return View(allAccessoriesToDisplay);
        }
    }
}