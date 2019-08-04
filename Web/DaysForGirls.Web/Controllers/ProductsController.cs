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
    public class ProductsController : Controller
    {
        private readonly IProductService productService;

        public ProductsController(
            IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet("/Products/All")]
        public async Task<IActionResult> All()
        {
            var allProducts = await this.productService
                .DisplayAll()
                .Select(product => new ProductDisplayAllViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Category = product.Category.Name,
                    ProductType = product.ProductType.Name,
                    Price = product.Price,
                    MainPicture = product.MainPicture.PictureUrl,
                    Quantity = product.Quantity.AvailableItems
                })
                .ToListAsync();

            return View(allProducts);
        }

        [HttpGet("/Products/Details/{Id}")]
        public async Task<IActionResult> Details(int id)
        {
            var productWithDetails = await this.productService
                .GetDetailsOfProductByIdAsync(id);

            ProductDetailsViewModel productDetails = new ProductDetailsViewModel
            {
                Id = productWithDetails.Id,
                Name = productWithDetails.Name,
                Category = productWithDetails.Category.Name,
                ProductType = productWithDetails.ProductType.Name,
                Colour = productWithDetails.Colour,
                Size = productWithDetails.Size,
                Description = productWithDetails.Description,
                Manufacturer = productWithDetails.Manufacturer.Name,
                Price = productWithDetails.Price,
                AvailableQuantity = productWithDetails.Quantity.AvailableItems,
                MainPicture = productWithDetails.MainPicture.PictureUrl
            };

            List<PictureDisplayAllViewModel> picDAVMs = new List<PictureDisplayAllViewModel>();

            foreach(var pic in productWithDetails.Pictures)
            {
                PictureDisplayAllViewModel pDAVM = new PictureDisplayAllViewModel
                {
                    ImageUrl = pic.PictureUrl
                };

                picDAVMs.Add(pDAVM);
            }

            productDetails.Pictures = picDAVMs;

            List<CustomerReviewAllViewModel> cRAVMs = new List<CustomerReviewAllViewModel>();

            if(productWithDetails.Reviews.Count() > 0)
            {
                foreach (var cR in productWithDetails.Reviews)
                {
                    CustomerReviewAllViewModel cRAVM = new CustomerReviewAllViewModel
                    {
                        Id = cR.Id,
                        Title = cR.Title,
                        Text = cR.Text,
                        Author = cR.Author.FirstName + " " + cR.Author.LastName
                    };

                    cRAVMs.Add(cRAVM);
                }
            }

            productDetails.Reviews = cRAVMs;

            //await Task.Delay(0);
            return View(productDetails);
        }

        //public async Task<IActionResult> AllAccessories()
        //{
        //    var allAccessories = this.productService
        //        .DisplayAll()
        //        .Select(a => new AccessoryDisplayAllViewModel
        //        {
        //            Id = a.Id,
        //            Name = a.Name,
        //            Category = a.Category.Name,
        //            Price = a.Price,
        //            MainPicture = a.MainPicture.PictureUrl,
        //            Quantity = a.Quantity.AvailableItems
        //        })
        //        .ToListAsync();

        //    return View(allAccessories);
        //}
    }
}