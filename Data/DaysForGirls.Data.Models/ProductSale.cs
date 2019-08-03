using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Data.Models
{
    public class ProductSale : BaseModel<int>
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int SaleId { get; set; }
        public Sale Sale { get; set; }
    }
}
