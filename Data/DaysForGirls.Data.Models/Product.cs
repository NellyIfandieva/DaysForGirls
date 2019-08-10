using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DaysForGirls.Data.Models
{
    public class Product : BaseModel<int>
    {
        private const string MinPrice = "0.00";
        private const string MaxPrice = "10000";

        public Product()
        {
            this.Pictures = new List<Picture>();
            this.Carts = new List<ProductCart>();
            this.Reviews = new List<CustomerReview>();
            this.Sales = new List<ProductSale>();
        }

        [Required]
        public string Name { get; set; }

        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Required]
        public string Description { get; set; }

        public ICollection<Picture> Pictures { get; set; }

        [Required]
        public string Colour { get; set; }

        [Required]
        public string Size { get; set; }

        [Range(typeof(decimal), MinPrice, MaxPrice)]
        public decimal Price { get; set; }

        public int ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }

        public int QuantityId { get; set; }
        public Quantity Quantity { get; set; }

        public ICollection<CustomerReview> Reviews { get; set; }

        public ICollection<ProductCart> Carts { get; set; }

        public ICollection<ProductSale> Sales { get; set; }

        public bool IsInSale { get; set; }

        public string OrderId { get; set; }
        public Order Order { get; set; }

        public bool IsDeleted { get; set; }
    }
}
