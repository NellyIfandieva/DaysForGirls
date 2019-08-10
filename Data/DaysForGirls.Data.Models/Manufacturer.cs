using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DaysForGirls.Data.Models
{
    public class Manufacturer : BaseModel<int>
    {
        [Index("SomeName", 2, IsUnique = true)]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }

        public bool IsDeleted { get; set; }
    }
}
