using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Web.ViewModels
{
    public class SaleAddProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public string ProductType { get; set; }

        public string Picture { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
