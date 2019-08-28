using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Web.ViewModels
{
    public class OrderDetailsViewModel
    {
        public string Id { get; set; }

        public string DeliveryEarlistDate { get; set; }

        public string DeliveryLatestDate { get; set; }

        public string IssuedOn { get; set; }

        public string UserIssuedTo { get; set; }

        public string OrderStatus { get; set; }

        public decimal TotalPrice { get; set; }

        public List<ProductInOrderViewModel> OrderedProducts { get; set; }
    }
}
