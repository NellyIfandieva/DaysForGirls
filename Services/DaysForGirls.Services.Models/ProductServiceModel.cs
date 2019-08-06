using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DaysForGirls.Services.Models
{
    public class ProductServiceModel
    {
        private const string MinPrice = "0.00";
        private const string MaxPrice = "10000.00";

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ProductTypeServiceModel ProductType { get; set; }

        public CategoryServiceModel Category { get; set; }

        [Required]
        public string Description { get; set; }

        public List<PictureServiceModel> Pictures { get; set; }

        [Required]
        public string Colour { get; set; }

        [Required]
        public string Size { get; set; }

        [Range(typeof(decimal), MinPrice, MaxPrice)]
        public decimal Price { get; set; }

        public ManufacturerServiceModel Manufacturer { get; set; }

        public QuantityServiceModel Quantity { get; set; }

        //public bool IsAvailable => this.Quantity.AvailableItems > 0;

        public List<CustomerReviewServiceModel> Reviews { get; set; }

        //public List<ProductCart> Carts { get; set; }

        //public OrderServiceModel Order { get; set; }
    }
}
