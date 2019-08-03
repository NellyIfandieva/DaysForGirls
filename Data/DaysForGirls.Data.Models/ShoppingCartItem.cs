using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Data.Models
{
    public class ShoppingCartItem : BaseModel<int>
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }

        public string ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
    }
}
