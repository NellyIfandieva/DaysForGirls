using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DaysForGirls.Data.Models
{
    public class Manufacturer : BaseModel<int>
    {
        [Required]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }

        public bool IsDeleted { get; set; }
    }
}
