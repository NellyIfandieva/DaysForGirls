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
            var allSpecialOccasionsProducts = await this.productService
                .AllOtherProducts().ToListAsync();

            List<ProductDisplayAllViewModel> productsToReturn = 
                new List<ProductDisplayAllViewModel>();

            foreach(var product in allSpecialOccasionsProducts)
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

        public async Task<IActionResult> Dresses()
        {
            var allOtherDresses = await this.productService
                .AllOtherDresses().ToListAsync();

            List<ProductDisplayAllViewModel> productsToReturn =
                new List<ProductDisplayAllViewModel>();

            foreach(var dress in allOtherDresses)
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

            return View(allOtherDresses);
        }

        public async Task<IActionResult> Suits()
        {
            var allOtherSuits = await this.productService
                .AllOtherSuits().ToListAsync();

            List<ProductDisplayAllViewModel> productsToReturn = 
                new List<ProductDisplayAllViewModel>();

            foreach(var suit in allOtherSuits)
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

                foreach(var pic in suit.Pictures)
                {
                    string url = pic.PictureUrl;
                    productPictures.Add(url);
                }

                pDAVM.Pictures = productPictures;
                productsToReturn.Add(pDAVM);
            }

            return View(productsToReturn);
        }

        public async Task<IActionResult> Accessories()
        {
            var allOtherAccessories = await this.productService
                .AllOtherAccessories().ToListAsync();

            List<ProductDisplayAllViewModel> productsToReturn =
                new List<ProductDisplayAllViewModel>();

            foreach (var accessory in allOtherAccessories)
            {
                var pDAVM = new ProductDisplayAllViewModel
                {
                    Id = accessory.Id,
                    Name = accessory.Name,
                    Category = accessory.Category.Name,
                    ProductType = accessory.ProductType.Name,
                    Price = accessory.Price,
                    Quantity = accessory.Quantity.AvailableItems
                };

                List<string> productPictures = new List<string>();

                foreach (var pic in accessory.Pictures)
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