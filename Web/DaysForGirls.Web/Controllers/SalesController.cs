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
        public async Task<IActionResult> Details([FromQuery]int id)
        {
            SaleServiceModel saleWithDetails = await this.saleService
                .GetSaleByIdAsync(id);

            SaleDetailsViewModel saleToDisplay = new SaleDetailsViewModel
            {
                Id = saleWithDetails.Id,
                Title = saleWithDetails.Title,
                EndsOn = saleWithDetails.EndsOn,
                Products = saleWithDetails.Products
                    .Select(product => new ProductInSaleViewModel
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Pictures = product.Pictures
                            .Select(pi => new PictureDisplayAllViewModel
                            {
                                Id = pi.Id,
                                ImageUrl = pi.PictureUrl,
                                ProductId = product.Id
                            })
                            .ToList(),
                        OldPrice = product.Price
                    }).ToList()
            };

            return View(saleToDisplay);
        }
    }
}
