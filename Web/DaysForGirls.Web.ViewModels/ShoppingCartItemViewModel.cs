namespace DaysForGirls.Web.ViewModels
{
    public class ShoppingCartItemViewModel
    {
        public int Id { get; set; }

        public ProductAsCartItemViewModel Product { get; set; }

        public int Quantity { get; set; }

        public string ShoppingCartId { get; set; }
    }
}
