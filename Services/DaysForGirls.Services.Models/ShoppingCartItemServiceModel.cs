using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Services.Models
{
    public class ShoppingCartItemServiceModel
    {
        public int Id { get; set; }

        public ProductAsShoppingCartItem Product { get; set; }

        public int Quantity { get; set; }

        //public ShoppingCartServiceModel ShoppingCart { get; set; }
    }
}
