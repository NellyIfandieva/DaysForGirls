using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Services.Models
{
    public class ProductSaleServiceModel
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public ProductServiceModel Product { get; set; }

        public int SaleId { get; set; }
        public SaleServiceModel Sale { get; set; }
    }
}
