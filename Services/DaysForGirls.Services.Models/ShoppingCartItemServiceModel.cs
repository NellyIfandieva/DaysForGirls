using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Services.Models
{
    public class ShoppingCartItemServiceModel
    {
        public ProductServiceModel Product { get; set; }

        public int Quantity { get; set; }

        public ShoppingCartServiceModel ShoppingCart { get; set; }
    }
}
