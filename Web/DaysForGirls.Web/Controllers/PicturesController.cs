namespace DaysForGirls.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using System.Threading.Tasks;
    using ViewModels;

    public class PicturesController : Controller
    {
        private readonly IPictureService pictureService;

        public PicturesController(
            IPictureService pictureService)
        {
            this.pictureService = pictureService;
        }

        [HttpGet("/Pictures/Display/{pictureId}")]
        public async Task<IActionResult> Display(int pictureId)
        {
            if (pictureId <= 0)
            {
                return Redirect("/Home/Error");
            }

            var picture = await this.pictureService
                .GetPictureByIdAsync(pictureId);

            if(picture == null)
            {
                return Redirect("/Home/Error");
            }

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