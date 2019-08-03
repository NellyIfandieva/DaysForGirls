using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Services.Models
{
    public class ProductDetailsServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ProductType { get; set; }

        public string Category { get; set; }

        public List<PictureServiceModel> Pictures { get; set; }

        public string Description { get; set; }

        public string Colour { get; set; }

        public string Size { get; set; }

        public decimal Price { get; set; }

        public string Manufacturer { get; set; }

        public List<CustomerReviewServiceModel> Reviews { get; set; }

        public int NumsAvailable { get; set; }
    }
}
