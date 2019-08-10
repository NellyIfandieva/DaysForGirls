using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Web.ViewModels
{
    public class ProductDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public string ProductType { get; set; }

        public string Colour { get; set; }

        public string Size { get; set; }

        public string Description { get; set; }

        public string Manufacturer { get; set; }

        public decimal Price { get; set; }

        public int AvailableQuantity { get; set; }

        public List<PictureDisplayAllViewModel> Pictures { get; set; }

        public List<CustomerReviewAllViewModel> Reviews { get; set; }
    }
}
