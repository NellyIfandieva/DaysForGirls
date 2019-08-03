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
            this.Products = new HashSet<Product>();
        }

        [Required]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }

        public bool IsDeleted { get; set; }
    }
}
