using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Services.Models
{
    public class SaleServiceModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public DateTime EndsOn { get; set; }

        public string Picture { get; set; }

        public bool IsActive { get; set; }

        public ProductServiceModel NewProduct { get; set; }

        public List<ProductServiceModel> Products { get; set; }

        public int ProductsCount => this.Products.Count;

        public bool IsDeleted { get; set; }
    }
}
