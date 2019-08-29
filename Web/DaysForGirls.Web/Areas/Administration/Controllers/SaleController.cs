﻿namespace DaysForGirls.Web.Areas.Administration.Controllers
{
    using Services;
    using Services.Models;
    using InputModels;
    using ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class SaleController : AdminController
    {
        private readonly ISaleService saleService;
        private readonly IProductService productService;
        private readonly IAdminService adminService;
        private readonly ICloudinaryService cloudinaryService;

        public SaleController(
            ISaleService saleService,
            IProductService productService,
            IAdminService adminService,
            ICloudinaryService cloudinaryService)
        {
            this.saleService = saleService;
            this.productService = productService;
            this.adminService = adminService;
            this.cloudinaryService = cloudinaryService;
        }

        [HttpGet("/Administration/Sale/Create")]
        public async Task<IActionResult> Create()
        {
            await Task.Delay(0);
            return View();
        }

        [HttpPost("/Administration/Sale/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaleCreateInputModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            string imageUrl = await this.cloudinaryService
                .UploadPictureForSaleAsync(model.Picture, model.Title);

            var saleServiceModel = new SaleServiceModel
            {
                Title = model.Title,
                EndsOn = model.EndsOn,
                Picture = imageUrl
            };

            string saleId = await this.saleService
                .CreateAsync(saleServiceModel);

            return Redirect("/Administration/Sale/All");
        }

        [HttpGet("/Administration/Sale/All")]
        public async Task<IActionResult> All()
        {
            var allSales = await this.saleService
                .DisplayAllAdmin()
                .Select(s => new SalesAllDisplayViewModelAdmin
                {
                    Id = s.Id,
                    Title = s.Title,
                    EndsOn = s.EndsOn.ToString("dddd, dd MMMM yyyy"),
                    Picture = s.Picture,
                    IsActive = s.IsActive,
                    ProductsCount = s.ProductsCount
                })
                .ToListAsync();

            if (allSales.Count() < 1)
            {
                return NotFound();
            }

            return View(allSales);
        }

        [HttpGet("/Administration/Sale/Details/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var sale = await this.saleService
                .GetSaleByIdAsync(id);

            if (sale == null)
            {
                return NotFound();
            }

            var saleToDisplay = new SaleDetailsAdminViewModel
            {
                Id = sale.Id,
                Picture = sale.Picture,
                Title = sale.Title,
                EndsOn = sale.EndsOn.ToString("dddd, dd MMMM yyyy"),
                IsActive = DateTime.UtcNow <= sale.EndsOn,
                Products = sale.Products
                    .Select(p => new ProductInSaleAdminViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Picture = p.Pictures.ElementAt(0).PictureUrl,
                        OldPrice = p.Price,
                        AvailableQuantity = p.Quantity.AvailableItems
                    }).ToList()
            };

            return View(saleToDisplay);
        }

        [HttpGet("/Administration/Sale/Edit/{saleId}")]
        public async Task<IActionResult> Edit(string saleId)
        {
            await Task.Delay(0);
            return View();
        }

        [HttpPost("/Administration/Sale/Edit/{saleId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string saleId, SaleEditInputModel model)
        {
            if(ModelState.IsValid == false)
            {
                return View(model);
            }

            string imageUrl = await this.cloudinaryService
                .UploadPictureForSaleAsync(model.Picture, model.Title);

            var saleToEdit = new SaleServiceModel
            {
                Id = saleId,
                Title = model.Title,
                EndsOn = model.EndsOn,
                Picture = imageUrl
            };

            bool saleIsEdited = await this.saleService
                .EditAsync(saleToEdit);

            return Redirect("/Administration/Sale/Details/" + saleId);
        }

        [HttpGet("/Administration/Sale/Delete/{saleId}")]
        public async Task<IActionResult> Delete(string saleId)
        {
            if(saleId == null)
            {
                return BadRequest();
            }

            bool isDeleted = await this.saleService
                .DeleteSaleById(saleId);

            return Redirect("/Administration/Sale/All");
        }
    }
}