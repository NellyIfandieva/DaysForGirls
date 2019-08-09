using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DaysForGirls.Services.Models
{
    public class ProductServiceModel
    {
        private const string MinPrice = "0.00";
        private const string MaxPrice = "10000.00";

        public ProductServiceModel()
        {
            this.Pictures = new List<PictureServiceModel>();
            this.Reviews = new List<CustomerReviewServiceModel>();
            this.Sales = new List<ProductSaleServiceModel>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public ProductTypeServiceModel ProductType { get; set; }

        public CategoryServiceModel Category { get; set; }

        public string Description { get; set; }

        public List<PictureServiceModel> Pictures { get; set; }

        public string Colour { get; set; }

        public string Size { get; set; }

        public decimal Price { get; set; }

        public ManufacturerServiceModel Manufacturer { get; set; }

        public QuantityServiceModel Quantity { get; set; }

        public List<CustomerReviewServiceModel> Reviews { get; set; }

        public bool IsDeleted { get; set; }

        public List<ProductSaleServiceModel> Sales { get; set; }

        public bool IsInSale => this.Sales.Count > 0;

        //public List<ProductCart> Carts { get; set; }

        //public OrderServiceModel Order { get; set; }
    }
}
