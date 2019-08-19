using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Services.Models
{
    public class ProductAsShoppingCartItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Colour { get; set; }

        public string Size { get; set; }

        public decimal Price { get; set; }

        public int AvailableItems { get; set; }

        public string MainPictureUrl { get; set; }

        public string SaleId { get; set; }

        public string ShoppingCartId { get; set; }
    }
}
