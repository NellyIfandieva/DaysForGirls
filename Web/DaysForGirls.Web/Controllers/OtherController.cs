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
            var allOtherProducts = await this.productService
                .GetAllProductsOfCategory("Other")
                .Select(p => new DisplayAllOfCategoryViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Picture = p.Picture.PictureUrl,
                    Price = p.Price.ToString("f2"),
                    IsInSale = p.IsInSale,
                    SaleId = p.SaleId
                })
                .ToListAsync();

            return View(allOtherProducts);
        }

        public async Task<IActionResult> Dresses()
        {
            var allOtherDresses = await this.productService
                .GetAllProductsOfTypeAndCategory("Dress", "Other")
                .Select(d => new DisplayAllOfCategoryAndTypeViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    Picture = d.Picture.PictureUrl,
                    Price = d.Price.ToString("f2")
                })
                .ToListAsync();

            return View(allOtherDresses);
        }

        public async Task<IActionResult> Suits()
        {
            var allOtherSuits = await this.productService
                .GetAllProductsOfTypeAndCategory("Suit", "Other")
                .Select(s => new DisplayAllOfCategoryAndTypeViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Picture = s.Picture.PictureUrl,
                    Price = s.Price.ToString("f2")
                })
                .ToListAsync();

            return View(allOtherSuits);
        }

        public async Task<IActionResult> Accessories()
        {
            var allOtherAccessories = await this.productService
                .GetAllProductsOfTypeAndCategory("Accessory", "Other")
                .Select(a => new DisplayAllOfCategoryAndTypeViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Picture = a.Picture.PictureUrl,
                    Price = a.Price.ToString("f2")
                })
                .ToListAsync();

            return View(allOtherAccessories);
        }
    }
}