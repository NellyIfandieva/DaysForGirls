using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DaysForGirls.Services.Models
{
    public class AccessoryServiceModel
    {
        private const string MinPrice = "0.00";
        private const string MaxPrice = "10000.00";

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public CategoryServiceModel Category { get; set; }

        [Required]
        public string Description { get; set; }

        public PictureServiceModel MainPicture { get; set; }

        [Required]
        public string Colour { get; set; }

        [Range(typeof(decimal), MinPrice, MaxPrice)]
        public decimal Price { get; set; }

        public ManufacturerServiceModel Manufacturer { get; set; }

        public QuantityServiceModel Quantity { get; set; }

        public List<CustomerReviewServiceModel> Reviews { get; set; }
    }
}
