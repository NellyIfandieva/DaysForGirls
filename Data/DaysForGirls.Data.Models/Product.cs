using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DaysForGirls.Data.Models
{
    public class Product : BaseModel<int>
    {
        private const string MinPrice = "0.00";
        private const string MaxPrice = "10000";
        private const string RequiredErrorMessage = "The field is required.";

        public Product()
        {
            this.Pictures = new List<Picture>();
            this.Reviews = new List<CustomerReview>();
        }

        [Required(ErrorMessage = RequiredErrorMessage)]
        public string Name { get; set; }

        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        public string Description { get; set; }

        public List<Picture> Pictures { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        public string Colour { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        public string Size { get; set; }

        [Range(typeof(decimal), MinPrice, MaxPrice)]
        public decimal Price { get; set; }

        public decimal SalePrice => this.Price - (0.3m * this.Price);

        public int ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }

        public int QuantityId { get; set; }
        public Quantity Quantity { get; set; }

        public List<CustomerReview> Reviews { get; set; }

        public string SaleId { get; set; }
        public Sale Sale { get; set; }

        public bool IsInSale { get; set; }

        public string ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }

        public string OrderId { get; set; }
        public Order Order { get; set; }

        public bool IsDeleted { get; set; }
    }
}
