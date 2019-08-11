using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Web.ViewModels
{
    public class ProductInSaleAdminViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Picture { get; set; }

        public decimal OldPrice { get; set; }

        public decimal NewPrice { get; set; }

        public int AvailableQuantity { get; set; }
    }
}
