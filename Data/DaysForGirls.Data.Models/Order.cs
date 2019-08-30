using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DaysForGirls.Data.Models
{
    public class Order : BaseModel<string>
    {
        //private const int MinOrderedQuantity = 1;
        //private const int MaxOrderedQuantity = 10;

        public Order()
        {
            this.IssuedOn = DateTime.UtcNow;
            this.OrderedProducts = new HashSet<OrderedProduct>();
        }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Issued On")]
        public DateTime IssuedOn { get; set; }

        public HashSet<OrderedProduct> OrderedProducts { get; set; }

        public decimal TotalPrice { get; set; }

        public string UserId { get; set; }
        public DaysForGirlsUser User { get; set; }

        public string OrderStatus { get; set; }

        public DateTime DeliveryEarliestDate => this.IssuedOn.AddDays(12);

        public DateTime DeliveryLatestDate => this.DeliveryEarliestDate.AddDays(5);

        //public int OrderStatusId { get; set; }
        //public OrderStatus OrderStatus { get; set; }

        public bool IsDeleted { get; set; }
    }
}