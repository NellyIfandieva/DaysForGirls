using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DaysForGirls.Data.Models
{
    public class CustomerReview : BaseModel<int>
    {
        public CustomerReview()
        {
            this.CreatedOn = DateTime.UtcNow;
        }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        public string AuthorId { get; set; }
        public DaysForGirlsUser Author { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; }
    }
}
