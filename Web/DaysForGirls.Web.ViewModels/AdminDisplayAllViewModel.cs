namespace DaysForGirls.Web.ViewModels
{
    public class AdminDisplayAllViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public string Picture { get; set; }

        public string Manufacturer { get; set; }

        public decimal Price { get; set; }

        public decimal SalePrice { get; set; }

        public int AvailableItems { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsInSale { get; set; }

        public string SaleId { get; set; }

        public decimal NewPrice => this.Price - (0.3m * this.Price);

        public string ShoppingCartId { get; set; }

        public string OrderId { get; set; }
    }
}
