using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DaysForGirls.Web.InputModels
{
    public class AccessoryCreateInputModel
    {
        private const string MinPrice = "0.00";
        private const string MaxPrice = "10000.00";
        private const int MinNumsAvailable = 0;
        private const int MaxNumsAvailable = 10;

        [Required(ErrorMessage = "Product Name is Required")]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public IFormFile MainPicture { get; set; }

        [Required]
        public string Colour { get; set; }

        [DataType(DataType.Currency)]
        [Range(typeof(decimal), MinPrice, MaxPrice)]
        public decimal Price { get; set; }

        [Required]
        public string Manufacturer { get; set; }


        [Range(MinNumsAvailable, MaxNumsAvailable)]
        public int Quantity { get; set; }
    }
}
