using System.Collections.Generic;

namespace DaysForGirls.Web.ViewModels
{
    public class ShoppingCartDisplayViewModel
    {
        public string Id { get; set; }

        public List<ShoppingCartItemViewModel> Items { get; set; }

        public decimal Total { get; set; }

        public string UserId { get; set; }
    }
}
