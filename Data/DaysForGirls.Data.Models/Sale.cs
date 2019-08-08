using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DaysForGirls.Data.Models
{
    public class Sale : BaseModel<int>
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Ends On")]
        public DateTime EndsOn { get; set; }

        public bool IsActive => DateTime.UtcNow <= this.EndsOn;

        [Required]
        public string Picture { get; set; }

        public ICollection<ProductSale> Products { get; set; }
    }
}
