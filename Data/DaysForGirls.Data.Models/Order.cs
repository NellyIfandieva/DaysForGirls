using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DaysForGirls.Data.Models
{
    public class Order : BaseModel<string>
    {
        private const int MinOrderedQuantity = 1;
        private const int MaxOrderedQuantity = 10;

        public Order()
        {
            this.IssuedOn = DateTime.UtcNow;
            this.Products = new HashSet<Product>();
        }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Issued On")]
        public DateTime IssuedOn { get; set; }

        public ICollection<Product> Products { get; set; }

        [Range(MinOrderedQuantity, MaxOrderedQuantity)]
        public int OrderedQuantity { get; set; }

        public decimal TotalPrice { get; set; }

        public string UserId { get; set; }
        public DaysForGirlsUser User { get; set; }

        public int OrderStatusId { get; set; }
        public OrderStatus OrderStatus { get; set; }

        public bool IsDeleted => this.User.IsDeleted = true;
    }
}