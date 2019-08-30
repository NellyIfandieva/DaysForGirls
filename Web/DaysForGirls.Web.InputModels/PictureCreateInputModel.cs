using Microsoft.AspNetCore.Http;

namespace DaysForGirls.Web.InputModels
{
    public class PictureCreateInputModel
    {
        public IFormFile Picture { get; set; }
    }
}
