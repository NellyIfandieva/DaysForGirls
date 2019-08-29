namespace DaysForGirls.Web.Controllers
{
    using Services;
    using ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

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
        public async Task<IActionResult> Display(int pictureId)
        {
            if(pictureId <= 0)
            {
                return BadRequest();
            }

            var picture = await this.pictureService
                .GetPictureByIdAsync(pictureId);

            var pictureToDisplay =
                new PictureDetailsViewModel
                {
                    Id = picture.Id,
                    PictureUrl = picture.PictureUrl,
                    ProductId = picture.ProductId
                };

            return View(pictureToDisplay);
        }
    }
}