using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DaysForGirls.Web.InputModels
{
    public class ProductCreateForSaleInputModel
    {
        private const string MinPrice = "0.00";
        private const string MaxPrice = "10000.00";
        private const int MinNumsAvailable = 0;
        private const int MaxNumsAvailable = 10;
        private const int MinSaleIdValue = 1;
        private const int MaxSaleIdValue = Int32.MaxValue;

        public ProductCreateForSaleInputModel()
        {
            this.Pictures = new HashSet<IFormFile>();
        }

        [Required(ErrorMessage = "Product Name is Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Product Type is Required")]
        public string ProductType { get; set; }

        [Required(ErrorMessage = "Category is Required")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Description is Required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Pictures are Required")]
        public ICollection<IFormFile> Pictures { get; set; }

        [Required(ErrorMessage = "Colour is Required")]
        public string Colour { get; set; }

        [Required(ErrorMessage = "Size is Required")]
        public string Size { get; set; }

        [DataType(DataType.Currency)]
        [Range(typeof(decimal), MinPrice, MaxPrice)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Manufacturer is Required")]
        public string Manufacturer { get; set; }

        [Range(MinNumsAvailable, MaxNumsAvailable)]
        public int Quantity { get; set; }

        //[Range(MinSaleIdValue, MaxSaleIdValue)]
        [Display(Name = "Sale Id")]
        public string SaleId { get; set; }
    }
}
