using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DaysForGirls.Web.InputModels
{
    public class CustomerReviewInputModel
    {
        private const int MinProductIdValue = 1;
        private const int MaxProductIdValue = int.MaxValue;

        [Required]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        [Range(MinProductIdValue, MaxProductIdValue)]
        public int ProductId { get; set; }

        //[Required]
        //public string AuthorsUsername { get; set; }
    }
}
