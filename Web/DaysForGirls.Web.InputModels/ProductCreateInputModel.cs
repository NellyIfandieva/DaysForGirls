using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DaysForGirls.Web.InputModels
{
    public class ProductCreateInputModel
    {
        private const string MinPrice = "0.00";
        private const string MaxPrice = "10000.00";
        private const int MinNumsAvailable = 0;
        private const int MaxNumsAvailable = 10;
        private const string RequiredFieldErrorMessage = "The field is required";

        public ProductCreateInputModel()
        {
            this.Pictures = new HashSet<IFormFile>();
        }

        [Required(ErrorMessage = RequiredFieldErrorMessage)]
        public string Name { get; set; }

        [Required(ErrorMessage = RequiredFieldErrorMessage)]
        public string ProductType { get; set; }

        [Required(ErrorMessage = RequiredFieldErrorMessage)]
        public string Category { get; set; }

        [Required(ErrorMessage = RequiredFieldErrorMessage)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Pictures is Required")]
        public ICollection<IFormFile> Pictures { get; set; }

        [Required(ErrorMessage = RequiredFieldErrorMessage)]
        public string Colour { get; set; }

        [Required(ErrorMessage = RequiredFieldErrorMessage)]
        public string Size { get; set; }

        [Required(ErrorMessage = RequiredFieldErrorMessage)]
        [DataType(DataType.Currency)]
        [Range(typeof(decimal), MinPrice, MaxPrice)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = RequiredFieldErrorMessage)]
        public string Manufacturer { get; set; }

        [Required(ErrorMessage = RequiredFieldErrorMessage)]
        [Range(MinNumsAvailable, MaxNumsAvailable)]
        public int Quantity { get; set; }

        [Display(Name = "Sale Title")]
        public string SaleTitle { get; set; }
    }
}
