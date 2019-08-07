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
                .GetAllProductsOfCategory("Prom").ToListAsync();

            List<ProductDisplayAllViewModel> productsToReturn = 
                new List<ProductDisplayAllViewModel>();

            foreach(var product in allPromProducts)
            {
                var pDAVM = new ProductDisplayAllViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Category = product.Category.Name,
                    ProductType = product.ProductType.Name,
                    Price = product.Price,
                    Quantity = product.Quantity.AvailableItems
                };

                List<string> productPictures = new List<string>();

                foreach (var pic in product.Pictures)
                {
                    string url = pic.PictureUrl;
                    productPictures.Add(url);
                }

                pDAVM.Pictures = productPictures;
                productsToReturn.Add(pDAVM);
            }

            return View(allPromProducts);
        }

        [HttpGet("/Proms/Dresses")]
        public async Task<IActionResult> Dresses()
        {
            var allPromDresses = await this.productService
                .GetAllProductsOfTypeAndCategory("Dress", "Prom").ToListAsync();

            List<ProductDisplayAllViewModel> productsToReturn =
                new List<ProductDisplayAllViewModel>();

            foreach (var product in allPromDresses)
            {
                var pDAVM = new ProductDisplayAllViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Category = product.Category.Name,
                    ProductType = product.ProductType.Name,
                    Price = product.Price,
                    Quantity = product.Quantity.AvailableItems
                };

                List<string> productPictures = new List<string>();

                foreach (var pic in product.Pictures)
                {
                    string url = pic.PictureUrl;
                    productPictures.Add(url);
                }

                pDAVM.Pictures = productPictures;
                productsToReturn.Add(pDAVM);
            }

            return View(productsToReturn);
        }

        [HttpGet("/Proms/Suits")]
        public async Task<IActionResult> Suits()
        {
            var allPromSuits = await this.productService
                .GetAllProductsOfTypeAndCategory("Suit", "Prom").ToListAsync();

            List<ProductDisplayAllViewModel> productsToReturn =
                new List<ProductDisplayAllViewModel>();

            foreach (var product in allPromSuits)
            {
                var pDAVM = new ProductDisplayAllViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Category = product.Category.Name,
                    ProductType = product.ProductType.Name,
                    Price = product.Price,
                    Quantity = product.Quantity.AvailableItems
                };

                List<string> productPictures = new List<string>();

                foreach (var pic in product.Pictures)
                {
                    string url = pic.PictureUrl;
                    productPictures.Add(url);
                }

                pDAVM.Pictures = productPictures;
                productsToReturn.Add(pDAVM);
            }

            return View(productsToReturn);
        }

        [HttpGet("/Proms/Accessories")]
        public async Task<IActionResult> Accessories()
        {
            var allPromAccessories = await this.productService
                .GetAllProductsOfTypeAndCategory("Accessory", "Prom").ToListAsync();

            List<ProductDisplayAllViewModel> productsToReturn =
                new List<ProductDisplayAllViewModel>();

            foreach (var product in allPromAccessories)
            {
                var pDAVM = new ProductDisplayAllViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Category = product.Category.Name,
                    ProductType = product.ProductType.Name,
                    Price = product.Price,
                    Quantity = product.Quantity.AvailableItems
                };

                List<string> productPictures = new List<string>();

                foreach (var pic in product.Pictures)
                {
                    string url = pic.PictureUrl;
                    productPictures.Add(url);
                }

                pDAVM.Pictures = productPictures;
                productsToReturn.Add(pDAVM);
            }

            return View(productsToReturn);
        }
    }
}