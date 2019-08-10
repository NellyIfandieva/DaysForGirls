using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaysForGirls.Data.Models;
using DaysForGirls.Services;
using DaysForGirls.Services.Models;
using DaysForGirls.Web.InputModels;
using DaysForGirls.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DaysForGirls.Web.Areas.Administration.Controllers
{
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
            return View();
        }

        [HttpPost("/Administration/Sale/Create")]
        public async Task<IActionResult> Create(SaleCreateInputModel model)
        {
            string imageUrl = await this.cloudinaryService
                .UploadPictureForSaleAsync(model.Picture, model.Title);


            SaleServiceModel saleServiceModel = new SaleServiceModel
            {
                Title = model.Title,
                EndsOn = model.EndsOn,
                Picture = imageUrl
            };

            int saleId = await this.saleService.Create(saleServiceModel);

            return Redirect("/Administration/Sale/All");
        }

        [HttpGet("/Administration/Sale/All")]
        public async Task<IActionResult> AllAdmin()
        {
            var allSales = this.saleService
                .DisplayAllAdmin()
                .Select(s => new SalesAllDisplayViewModelAdmin
                {
                    Id = s.Id,
                    Title = s.Title,
                    EndsOn = s.EndsOn,
                    Picture = s.Picture,
                    IsActive = s.IsActive,
                    Products = s.Products
                        .Select(p => new ProductDisplayAllViewModel
                        {
                            Id = p.Product.Id
                        }).ToList()
                })
                .ToList();

            return View(allSales);
        }

        [HttpGet()]
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
                        Id = p.Product.Id,
                        Name = p.Product.Name,
                        Picture = p.Product.Pictures.ElementAt(0).PictureUrl,
                        OldPrice = p.Product.Price,
                        Quantity = p.Product.Quantity.AvailableItems
                    }).ToList()
            };

            await Task.Delay(0);
            return View(saleToDisplay);
        }

        public async Task<IActionResult> Edit(int saleId)
        {
            throw new NotImplementedException();
            //return Redirect("/Administration/Sale/Details/{saleId}");
        }

        [HttpGet("/Administration/Sale/AddProductToSale/{saleId}")]
        public async Task<IActionResult> AddProductToSale(int saleId)
        {
                var allProducts = await this.adminService
                    .DisplayAll()
                    .Where(p => p.IsInSale == false)
                    .ToListAsync();

            this.ViewData["allProducts"] = allProducts
                .Select(p => new ProductAddToSaleViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Picture = p.Picture.PictureUrl,
                    Price = p.Price.ToString("f2")
                })
                .ToList();

            return View();
        }

        [HttpPost("/Administration/Sale/AddProductToSale/{saleId}")]
        public async Task<IActionResult> AddProductToSale(SaleAddProductInputModel model)
        {
            var saleToAddTo = await this.saleService.GetSaleByIdAsync(model.SaleId);

            var productToAdd = await this.adminService
                .GetProductByNameAsync(model.ProductName);

            saleToAddTo.NewProduct = new ProductSaleServiceModel
            {
                ProductId = productToAdd.Id,
                SaleId = model.SaleId
            };

            bool saleAddedProduct = await this.saleService.AddProductToSale(saleToAddTo);
            bool productAddedSale = await this.adminService.AddProductToSaleAsync(productToAdd.Id, model.SaleId);

            return Redirect("/Administration/Sale/Details/{saleId}");
        }
    }
}