using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Web.ViewModels
{
    public class ShoppingCartItemViewModel
    {
        public List<ProductInCartViewModel> Product { get; set; }

        public string ShoppingCartId { get; set; }
    }
}
