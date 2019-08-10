using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Services.Models
{
    public class ManufacturerServiceModel
    {
        public ManufacturerServiceModel()
        {
            this.Products = new List<ProductServiceModel>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public LogoServiceModel Logo { get; set; }

        public List<ProductServiceModel> Products { get; set; }

        public bool IsDeleted { get; set; }
    }
}
