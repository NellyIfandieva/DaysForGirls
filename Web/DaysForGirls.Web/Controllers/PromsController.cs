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
            var allPromProducts = await this.productService
                .AllPromProducts()
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

            return View(allPromProducts);
        }

        [HttpGet("/Proms/Dresses")]
        public async Task<IActionResult> Dresses()
        {
            var allPromDresses = await this.productService
                .AllPromDresses()
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

            return View(allPromDresses);
        }

        [HttpGet("/Proms/Suits")]
        public async Task<IActionResult> Suits()
        {
            var allPromSuits = await this.productService
                .AllPromSuits()
                .Select(d => new ProductDisplayAllViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    Price = d.Price,
                    Category = d.Category.Name,
                    ProductType = d.ProductType.Name,
                    MainPicture = d.MainPicture.PictureUrl,
                    Quantity = d.Quantity.AvailableItems
                })
                .ToListAsync();

            return View(allPromSuits);
        }

        [HttpGet("/Proms/Accessories")]
        public async Task<IActionResult> Accessories()
        {
            var allPromAccessories = await this.productService
                .AllPromAccessories()
                .Select(d => new ProductDisplayAllViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    Price = d.Price,
                    Category = d.Category.Name,
                    MainPicture = d.MainPicture.PictureUrl,
                    Quantity = d.Quantity.AvailableItems
                })
                .ToListAsync();

            return View(allPromAccessories);
        }
    }
}