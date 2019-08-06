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

        public WeddingsController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet("/Weddings/All")]
        public async Task<IActionResult> All()
        {
            var allWeddingProducts = await this.productService
                .AllWeddingProducts().ToListAsync();

            List<ProductDisplayAllViewModel> productsToReturn = 
                new List<ProductDisplayAllViewModel>();

            foreach(var product in allWeddingProducts)
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

                foreach(var pic in product.Pictures)
                {
                    string url = pic.PictureUrl;
                    productPictures.Add(url);
                }

                pDAVM.Pictures = productPictures;
                productsToReturn.Add(pDAVM);
            }

            return View(productsToReturn);
        }
        

        [HttpGet("/Weddings/Dresses")]
        public async Task<IActionResult> Dresses()
        {
            var allWeddingDresses = await this.productService
                .AllWeddingDresses().ToListAsync();

            List<ProductDisplayAllViewModel> productsToReturn = 
                new List<ProductDisplayAllViewModel>();

            foreach(var dress in allWeddingDresses)
            {
                var pDAVM = new ProductDisplayAllViewModel
                {
                    Id = dress.Id,
                    Name = dress.Name,
                    Category = dress.Category.Name,
                    ProductType = dress.ProductType.Name,
                    Price = dress.Price,
                    Quantity = dress.Quantity.AvailableItems
                };

                List<string> productPictures = new List<string>();

                foreach(var pic in dress.Pictures)
                {
                    string url = pic.PictureUrl;
                    productPictures.Add(url);
                }

                pDAVM.Pictures = productPictures;
                productsToReturn.Add(pDAVM);
            }

            return View(productsToReturn);
        }

        [HttpGet("/Weddings/Suits")]
        public async Task<IActionResult> Suits()
        {
            var allWeddingSuits = await this.productService
                .AllWeddingSuits().ToListAsync();

            List<ProductDisplayAllViewModel> productsToReturn =
                new List<ProductDisplayAllViewModel>();

            foreach (var suit in allWeddingSuits)
            {
                var pDAVM = new ProductDisplayAllViewModel
                {
                    Id = suit.Id,
                    Name = suit.Name,
                    Category = suit.Category.Name,
                    ProductType = suit.ProductType.Name,
                    Price = suit.Price,
                    Quantity = suit.Quantity.AvailableItems
                };

                List<string> productPictures = new List<string>();

                foreach (var pic in suit.Pictures)
                {
                    string url = pic.PictureUrl;
                    productPictures.Add(url);
                }

                pDAVM.Pictures = productPictures;
                productsToReturn.Add(pDAVM);
            }

            return View(productsToReturn);
        }

        [HttpGet("/Weddings/Accessories")]
        public async Task<IActionResult> Accessories()
        {
            var allWeddingAccessories = await this.productService
                .AllWeddingAccessories()
                .ToListAsync();

            List<ProductDisplayAllViewModel> productsToReturn =
                new List<ProductDisplayAllViewModel>();

            foreach(var accessory in allWeddingAccessories)
            {
                ProductDisplayAllViewModel pDAVM = new ProductDisplayAllViewModel
                {
                    Id = accessory.Id,
                    Name = accessory.Name,
                    Price = accessory.Price,
                    Category = accessory.Category.Name
                };

                List<string> productPictures = new List<string>();

                foreach(var pic in accessory.Pictures)
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