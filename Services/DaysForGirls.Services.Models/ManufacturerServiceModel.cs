using System.Collections.Generic;

namespace DaysForGirls.Services.Models
{
    public class ManufacturerServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public LogoServiceModel Logo { get; set; }

        public List<ProductServiceModel> Products { get; set; }

        public bool IsDeleted { get; set; }

        public int ProductsCount => this.Products.Count;
    }
}
