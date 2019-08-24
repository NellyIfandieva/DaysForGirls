using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Services.Models
{
    public class DisplayAllOfCategoryAndTypeServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public PictureServiceModel Picture { get; set; }

        public decimal Price { get; set; }

        public int AvailableItems { get; set; }

        public bool IsInSale { get; set; }

        public string SaleId { get; set; }

        public string ShoppingCartId { get; set; }

        public string OrderId { get; set; }
    }
}
