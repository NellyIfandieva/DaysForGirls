using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DaysForGirls.Web.InputModels
{
    public class SaleCreateInputModel
    {
        private const string RequiredFieldErrorMessage = "The field is required";
        private const string RequiredPictureErrorMessage = "You need to upload a picture";

        [Required(ErrorMessage = RequiredFieldErrorMessage)]
        public string Title { get; set; }

        [Required(ErrorMessage = RequiredFieldErrorMessage)]
        [DataType(DataType.DateTime)]
        [Display(Name = "Ends On")]
        public DateTime EndsOn { get; set; }

        [Required(ErrorMessage = RequiredPictureErrorMessage)]
        public IFormFile Picture { get; set; }
    }
}
