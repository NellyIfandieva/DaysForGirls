using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Services.Models
{
    public class ProductDisplayAllServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public PictureServiceModel Picture { get; set; }

        public int AvailableItems { get; set; }

        public string SaleId { get; set; }

        public string ShoppingCartId { get; set; }
    }
}
