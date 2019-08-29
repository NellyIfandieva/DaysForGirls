using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Web.ViewModels
{
    public class ProductDisplayAllViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Price { get; set; }

        public decimal SalePrice { get; set; }

        public int AvailableItems { get; set; }

        public string Picture { get; set; }

        public string SaleId { get; set; }

        public string ShoppingCartId { get; set; }

        public string OrderId { get; set; }
    }
}
