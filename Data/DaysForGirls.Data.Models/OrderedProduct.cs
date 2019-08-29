using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Data.Models
{
    public class OrderedProduct : BaseModel<int>
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductColour { get; set; }

        public string ProductSize { get; set; }

        public decimal ProductPrice { get; set; }

        public decimal ProductSalePrice => this.ProductPrice - (0.3m * this.ProductPrice);

        public string ProductPicture { get; set; }

        public int ProductQuantity { get; set; }
    }
}
