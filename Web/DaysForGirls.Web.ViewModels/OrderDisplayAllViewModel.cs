using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Web.ViewModels
{
    public class OrderDisplayAllViewModel
    {
        public int Number { get; set; }

        public string Id { get; set; }

        public string IssuedOn { get; set; }

        public List<OrderedProductsDisplayAllViewModel> ProductsInOrder { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; }
    }
}
