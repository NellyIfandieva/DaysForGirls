﻿namespace DaysForGirls.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Services;
    using System.Linq;
    using System.Threading.Tasks;
    using ViewModels;

    public class ProductsController : Controller
    {
        private readonly IProductService productService;
        private readonly IAdminService adminService;
        private readonly ISaleService saleService;

        public ProductsController(
            IProductService productService,
            IAdminService adminService,
            ISaleService saleService)
        {
            this.productService = productService;
            this.adminService = adminService;
            this.saleService = saleService;
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
                    AvailableItems = p.AvailableItems,
                    Picture = p.Picture.PictureUrl,
                    SaleId = p.SaleId,
                    ShoppingCartId = p.ShoppingCartId,
                    OrderId = p.OrderId
                }).ToListAsync();

            return View(allProducts);
        }

        [HttpGet("/Products/Details/{productId}")]
        public async Task<IActionResult> Details(int productId)
        {
            if (productId <= 0)
            {
                return Redirect("/Home/Error");
            }

            var productFromDb = await this.adminService
                .GetProductByIdAsync(productId);

            if (productFromDb == null)
            {
                return Redirect("/Home/Error");
            }

            string saleTitle = null;

            if (productFromDb.SaleId != null)
            {
                var sale = await this.saleService.GetSaleByIdAsync(productFromDb.SaleId);

                if(sale == null)
                {
                    return Redirect("/Home/Error");
                }

                saleTitle = sale.Title;
            }

            var productToDisplay = new ProductDetailsGeneralUserViewModel
            {
                Id = productFromDb.Id,
                Name = productFromDb.Name,
                Description = productFromDb.Description,
                Colour = productFromDb.Colour,
                Size = productFromDb.Size,
                Price = productFromDb.Price.ToString("f2"),
                AvailableItems = productFromDb.Quantity.AvailableItems,
                Pictures = productFromDb.Pictures
                    .Select(pic => new PictureDetailsViewModel
                    {
                        Id = pic.Id,
                        PictureUrl = pic.PictureUrl
                    })
                    .ToList(),
                ManufacturerId = productFromDb.Manufacturer.Id,
                ManufacturerName = productFromDb.Manufacturer.Name,
                Reviews = productFromDb.Reviews
                    .Select(r => new CustomerReviewAllViewModel
                    {
                        Id = r.Id,
                        Title = r.Title,
                        Text = r.Text,
                        DateCreated = r.CreatedOn,
                        AuthorUsername = r.AuthorUsername
                    })
                    .ToList(),
                SaleId = productFromDb.SaleId,
                SaleTitle = saleTitle,
                ShoppingCartId = productFromDb.ShoppingCartId,
                OrderId = productFromDb.OrderId
            };

            return View(productToDisplay);
        }
    }
}