using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DaysForGirls.Web.InputModels
{
    public class CustomerReviewInputModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public string Product { get; set; }

        [Required]
        public string AuthorsUsername { get; set; }
    }
}
