using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Web.ViewModels
{
    public class ProductInSaleViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string MainPicture { get; set; }

        public decimal OldPrice { get; set; }

        public decimal NewPrice => this.OldPrice - (0.3m * this.OldPrice);

        public int AvailableItems { get; set; }

        public string ShoppingCartId { get; set; }

        public string OrderId { get; set; }
    }
}
