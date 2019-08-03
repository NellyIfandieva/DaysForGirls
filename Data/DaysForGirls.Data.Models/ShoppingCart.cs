using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Data.Models
{
    public class ShoppingCart : BaseModel<string>
    {
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}
