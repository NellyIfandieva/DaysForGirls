using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Web.ViewModels
{
    public class AdminOrdersDisplayAllViewModel
    {
        public int Number { get; set; }
        public string Id { get; set; }

        public string IssuedOn { get; set; }

        public List<OrderedProductsDisplayAllViewModel> OrderedProducts { get; set; }

        public decimal TotalPrice { get; set; }

        public string UserId { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public string OrderStatus { get; set; }

        public bool IsDeleted { get; set; }
    }
}
