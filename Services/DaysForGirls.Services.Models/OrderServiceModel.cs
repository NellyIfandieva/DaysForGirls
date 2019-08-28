using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DaysForGirls.Services.Models
{
    public class OrderServiceModel
    {
        //private const int MinOrderedQuantity = 1;
        //private const int MaxOrderedQuantity = 10;

        public string Id { get; set; }

        public DateTime IssuedOn { get; set; }

        public string IssuedTo { get; set; }

        public List<OrderedProductServiceModel> OrderedProducts { get; set; }

        public decimal TotalPrice { get; set; }

        public DaysForGirlsUserServiceModel User { get; set; }

        public string DeliveryEarlistDate { get; set; }

        public string DeliveryLatestDate { get; set; }

        public string OrderStatus { get; set; }

        public bool IsDeleted { get; set; }
    }
}
