using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DaysForGirls.Web.InputModels
{
    public class ManufacturerCreateInputModel
    {
        private const string RequiredErrorMessage = "The field is required";
        private const string RequiredPictureErrorMessage = "You need to upload a picture";

        [Required(ErrorMessage = RequiredErrorMessage)]
        public string Name { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        public string Description { get; set; }

        [Required(ErrorMessage = RequiredPictureErrorMessage)]
        public IFormFile Logo { get; set; }
    }
}
