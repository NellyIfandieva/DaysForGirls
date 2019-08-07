using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DaysForGirls.Data.Models
{
    public class Sale : BaseModel<int>
    {
        public Sale()
        {
            this.Products = new HashSet<ProductSale>();
        }

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
