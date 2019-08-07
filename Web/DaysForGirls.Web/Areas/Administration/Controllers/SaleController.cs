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
        private readonly ICloudinaryService cloudinaryService;

        public SaleController(
            ISaleService saleService,
            IProductService productService,
            ICloudinaryService cloudinaryService)
        {
            this.saleService = saleService;
            this.productService = productService;
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
            var allSales = await this.saleService
                .DisplayAllAdmin()
                .Select(s => new SalesAllDisplayViewModelAdmin
                {
                    Id = s.Id,
                    Title = s.Title,
                    EndsOn = s.EndsOn,
                    Picture = s.Picture,
                    IsActive = s.IsActive
                })
                .ToListAsync();

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
                        Id = p.Id,
                        Name = p.Name,
                        Pictures = p.Pictures
                            .Select(pic => new PictureDisplayAllViewModel
                            {
                                Id = pic.Id,
                                ImageUrl = pic.PictureUrl,
                                ProductId = p.Id
                            }).ToList(),
                        OldPrice = p.Price,
                        Quantity = p.Quantity.AvailableItems
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

        [HttpGet("/Administration/Sale/AddExistingProduct")]
        public async Task<IActionResult> AddExistingProduct()
        {
            await Task.Delay(0);
            return View();
        }

        [HttpPost()]
        public async Task<IActionResult> AddExistingProduct(int saleId, int productId)
        {
            SaleServiceModel sale = await this.saleService
                .GetSaleByIdAsync(saleId);

            ProductServiceModel product = await this.productService
                .GetProductDetailsById(productId);

            sale.Products.Add(product);
            
            //TODO back to service to update the sale
            //then savechagesasync();

            return Redirect("/Sales/All");
        }

        [HttpGet("/Administration/Sale/AddNewProduct")]
        public async Task<IActionResult> AddNewProduct()
        {
            await Task.Delay(0);
            return View();
        }

        [HttpPost("/Administration/Sale/AddNewProduct")]
        public async Task<IActionResult> AddNewProduct(int saleId, ProductCreateInputModel model)
        {
            ProductServiceModel product = new ProductServiceModel
            {
                Name = model.Name,
                Category = new CategoryServiceModel
                {
                    Name = model.Category
                },
                ProductType = new ProductTypeServiceModel
                {
                    Name = model.ProductType
                },
                //MainPicture = new PictureServiceModel
                //{
                //    PictureUrl = model.MainPicture
                //},
                //Pictures = new 
                Price = model.Price,
                Colour = model.Colour,
                Size = model.Size,
                Manufacturer = new ManufacturerServiceModel
                {
                    Name = model.Manufacturer
                },
                Quantity = new QuantityServiceModel
                {
                    AvailableItems = model.Quantity
                }
            };

            int productId = await this.productService.Create(product);
            bool isAdded = await this.saleService.AddProductToSale(saleId, productId);

            return Redirect("/Sales/Details/");
        }
    }
}