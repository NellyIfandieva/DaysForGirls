using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace DaysForGirls.Web.InputModels
{
    public class SaleCreateInputModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Ends On")]
        public DateTime EndsOn { get; set; }

        [Required]
        public IFormFile Picture { get; set; }
    }
}
