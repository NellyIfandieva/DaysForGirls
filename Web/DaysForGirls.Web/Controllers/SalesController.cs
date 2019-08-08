using DaysForGirls.Services;
using DaysForGirls.Services.Models;
using DaysForGirls.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaysForGirls.Web.Controllers
{
    public class SalesController : Controller
    {
        private readonly ISaleService saleService;

        public SalesController(ISaleService saleService)
        {
            this.saleService = saleService;
        }

        [HttpGet("/Sales/All")]
        public async Task<IActionResult> All()
        {
            var allSales = await this.saleService
                .DisplayAll()
                .Select(sale => new SaleDisplayAllViewModel
                {
                    Id = sale.Id,
                    Title = sale.Title,
                    Picture = sale.Picture,
                    EndsOn = sale.EndsOn
                })
                .ToListAsync();

            return View(allSales);
        }

        [HttpGet("/Sales/Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var sale = await this.saleService.GetSaleByIdAsync(id);

            var saleToDisplay = new SaleDetailsViewModel
            {
                Id = sale.Id,
                Title = sale.Title,
                EndsOn = sale.EndsOn,
                IsValid = sale.IsActive,
                Products = sale.Products
                    .Select(p => new ProductInSaleViewModel
                    {
                        Id = p.Id,
                        Name = p.Product.Name,
                        Pictures = p.Product.Pictures
                            .Select(pic => new PictureDisplayAllViewModel
                            {
                                Id = pic.Id,
                                ImageUrl = pic.PictureUrl,
                                ProductId = p.Id
                            }).ToList(),
                        OldPrice = p.Product.Price,
                        Quantity = p.Product.Quantity.AvailableItems
                    }).ToList()
            };

            await Task.Delay(0);
            return View(saleToDisplay);
        }
    }
}
