using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Web.ViewModels
{
    public class OrderDisplayViewModel
    {
        public string Id { get; set; }

        public string IssuedOn { get; set; }

        public List<OrderedProductDisplayViewModel> OrderedProducts { get; set; }

        public decimal TotalPrice { get; set; }

        public string UserFullName { get; set; }

        public string DeliveryEarliestDate { get; set; }

        public string DeliveryLatestDate { get; set; }

        public string OrderStatus { get; set; }

        public bool IsDeleted { get; set; }
    }
}
