using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Services.Models
{
    public class ProductSaleServiceModel
    {
        public int Id { get; set; }

        public ProductServiceModel Product { get; set; }

        public SaleServiceModel Sale { get; set; }
    }
}
