﻿using System;
using System.ComponentModel.DataAnnotations;

namespace DaysForGirls.Data.Models
{
    public class CustomerReview : BaseModel<int>
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; }

        public string AuthorId { get; set; }
        public DaysForGirlsUser Author { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public bool IsDeleted { get; set; }
    }
}
