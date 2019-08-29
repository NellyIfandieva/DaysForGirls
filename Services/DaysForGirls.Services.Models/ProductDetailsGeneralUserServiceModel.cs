using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Services.Models
{
    public class ProductDetailsGeneralUserServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Colour { get; set; }

        public string Size { get; set; }

        public string Price { get; set; }

        public decimal SalePrice { get; set; }

        public int AvailableItems { get; set; }

        public List<PictureServiceModel> Pictures { get; set; }

        public ManufacturerServiceModel Manufacturer { get; set; }

        public List<CustomerReviewServiceModel> Reviews { get; set; }

        public bool IsInSale { get; set; }

        public string SaleId { get; set; }
    }
}
