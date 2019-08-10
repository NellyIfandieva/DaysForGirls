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
                .Select(p => new ProductDisplayAllViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price.ToString("f2"),
                    Picture = p.Picture.PictureUrl
                }).ToListAsync();

            return View(allProducts);
        }

        [HttpGet("/Products/Details/{Id}")]
        public async Task<IActionResult> Details(int id)
        {
            var productWithDetails = await this.productService
                .GetProductByIdAsync(id);

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
                AvailableQuantity = productWithDetails.Quantity.AvailableItems
            };

            var pictures = productWithDetails.Pictures
                .Select(p => new PictureDisplayAllViewModel
                {
                    Id = p.Id,
                    ImageUrl = p.PictureUrl,
                    ProductId = p.ProductId
                }).ToList();

            productDetails.Pictures = pictures
                .Select(p => new PictureDisplayAllViewModel
                {
                    Id = p.Id,
                    ImageUrl = p.ImageUrl
                }).ToList();

            var reviews = productWithDetails.Reviews
                .Select(r => new CustomerReviewAllViewModel
                {
                    Id = r.Id,
                    Title = r.Title,
                    Text = r.Text,
                    Author = r.AuthorUsername,
                    DateCreated = r.CreatedOn
                })
                .ToList();

            productDetails.Reviews = reviews;

            return View(productDetails);
        }
    }
}