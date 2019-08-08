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

        public string Name { get; set; }

        public ProductTypeServiceModel ProductType { get; set; }

        public CategoryServiceModel Category { get; set; }

        public string Description { get; set; }

        public List<PictureServiceModel> Pictures { get; set; }

        [Required]
        public string Colour { get; set; }

        public string Size { get; set; }

        public decimal Price { get; set; }

        public ManufacturerServiceModel Manufacturer { get; set; }

        public QuantityServiceModel Quantity { get; set; }

        public List<CustomerReviewServiceModel> Reviews { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsInSale { get; set; }

        //public List<ProductCart> Carts { get; set; }

        //public OrderServiceModel Order { get; set; }
    }
}
