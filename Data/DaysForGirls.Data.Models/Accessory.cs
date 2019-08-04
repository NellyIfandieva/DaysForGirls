using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DaysForGirls.Data.Models
{
    public class Accessory// : BaseModel<int>
    {
        private const string MinPrice = "0.00";
        private const string MaxPrice = "10000";

        public Accessory()
        {
            this.Reviews = new HashSet<CustomerReview>();
            this.Carts = new HashSet<AccessoryCart>();
        }

        [Required]
        public string Name { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string MainPicture { get; set; }

        [Required]
        public string Colour { get; set; }

        [Range(typeof(decimal), MinPrice, MaxPrice)]
        public decimal Price { get; set; }

        public int ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }

        public int QuantityId { get; set; }
        public Quantity Quantity { get; set; }

        public ICollection<CustomerReview> Reviews { get; set; }

        public ICollection<AccessoryCart> Carts { get; set; }

        public string OrderId { get; set; }
        public Order Order { get; set; }

        public bool IsDeleted { get; set; }
    }
}
