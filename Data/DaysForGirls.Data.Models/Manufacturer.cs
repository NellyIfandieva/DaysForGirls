using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DaysForGirls.Data.Models
{
    public class Manufacturer : BaseModel<int>
    {
        public Manufacturer()
        {
            this.Products = new List<Product>();
        }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public Logo Logo { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
