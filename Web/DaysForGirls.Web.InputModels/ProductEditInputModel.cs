using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DaysForGirls.Web.InputModels
{
    public class ProductEditInputModel
    {
        private const string MinPrice = "0.00";
        private const string MaxPrice = "10000";

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ProductType { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Description { get; set; }

        public List<string> CurrentPictures { get; set; }

        public IFormFile NewPicture { get; set; }

        [Required]
        public string Colour { get; set; }

        [Required]
        public string Size { get; set; }

        [Range(typeof(decimal), MinPrice, MaxPrice)]
        public decimal Price { get; set; }

        public string Manufacturer { get; set; }

        public int Quantity { get; set; }
    }
}
