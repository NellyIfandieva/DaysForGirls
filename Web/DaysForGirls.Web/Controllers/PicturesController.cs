using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaysForGirls.Services;
using DaysForGirls.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DaysForGirls.Web.Controllers
{
    public class PicturesController : Controller
    {
        private readonly IPictureService pictureService;
        private readonly IProductService productService;

        public PicturesController(
            IPictureService pictureService,
            IProductService productService)
        {
            this.pictureService = pictureService;
            this.productService = productService;
        }

        [HttpGet("/Pictures/Display/{id}")]
        public async Task<IActionResult> Display(int id)
        {
            var picture = await this.pictureService
                .GetPictureByIdAsync(id);

            PictureDetailsViewModel pictureToDisplay =
                new PictureDetailsViewModel
                {
                    Id = picture.Id,
                    PictureUrl = picture.PictureUrl,
                    ProductId = picture.ProductId
                };
            var product = await this.productService
                .GetProductDetailsById(picture.ProductId);

            this.ViewData["productName"] = product.Name;

            return View(pictureToDisplay);
        }
    }
}