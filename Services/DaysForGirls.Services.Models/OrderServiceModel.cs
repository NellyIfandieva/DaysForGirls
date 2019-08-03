using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DaysForGirls.Services.Models
{
    public class OrderServiceModel
    {
        private const int MinOrderedQuantity = 1;
        private const int MaxOrderedQuantity = 10;

        public string Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Issued On")]
        public DateTime IssuedOn { get; set; }

        public ProductServiceModel Product { get; set; }

        [Range(MinOrderedQuantity, MaxOrderedQuantity)]
        public int OrderedQuantity { get; set; }

        public decimal TotalPrice { get; set; }

        public DaysForGirlsUserServiceModel User { get; set; }

        public OrderStatusServiceModel OrderStatus { get; set; }

        public bool IsDeleted => this.User.IsDeleted = true;
    }
}
