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
            await Task.Delay(0);
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

            bool isCreated = await this.saleService.Create(saleServiceModel);

            //TODO implement the case isCreated == false

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

        public async Task<IActionResult> Edit(int saleId)
        {
            throw new NotImplementedException();
            //return Redirect("/Administration/Sale/Details/{saleId}");
        }

        [HttpGet("/Administration/Sale/AddProductToSale/{saleId}")]
        public async Task<IActionResult> AddProductToSale(int saleId)
        {
            var allProducts = this.adminService
                .DisplayAll().ToList();

            this.ViewData["allProducts"] = allProducts
                .Select(p => new SaleAddProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Category = p.Category.Name,
                    Picture = p.Picture.PictureUrl,
                    Price = p.Price,
                    Manufacturer = p.Manufacturer.Name
                })
                .OrderBy(p => p.Name);

            return View();
        }

        [HttpPost()]
        public async Task<IActionResult> AddProductToSale(SaleAddProductInputModel model)
        {
            var saleToAddTo = await this.saleService.GetSaleByIdAsync(model.SaleId);

            var productToAdd = await this.adminService.GetProductByIdAsync(model.ProductId);

            ProductSaleServiceModel productSale = new ProductSaleServiceModel
            {
                Product = productToAdd,
                Sale = saleToAddTo
            };
            
            saleToAddTo.Products.Add(productSale);

            bool saleAddedProduct = await this.saleService.AddProductToSale(model.SaleId, model.ProductId);
            bool productAddedSale = await this.adminService.AddProductToSaleAsync(model.ProductId, model.SaleId);

            return Redirect("/Administration/Sale/Details/{saleId}");
        }
    }
}