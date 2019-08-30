using System.Collections.Generic;

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

        public int ManufacturerId { get; set; }

        public string Manufacturer { get; set; }

        public decimal Price { get; set; }

        public decimal SalePrice { get; set; }

        public int AvailableQuantity { get; set; }

        public List<PictureDisplayAllViewModel> Pictures { get; set; }

        public bool IsDeleted { get; set; }

        public List<CustomerReviewAllViewModel> Reviews { get; set; }

        public string SaleId { get; set; }

        public string SaleName { get; set; }

        public string ShoppingCartId { get; set; }

        public string OrderId { get; set; }
    }
}
