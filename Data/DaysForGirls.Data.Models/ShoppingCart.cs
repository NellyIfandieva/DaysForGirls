using System.Collections.Generic;

namespace DaysForGirls.Data.Models
{
    public class ShoppingCart : BaseModel<string>
    {
        public ShoppingCart()
        {
            this.ShoppingCartItems = new List<ShoppingCartItem>();
        }

        public string UserId { get; set; }
        public DaysForGirlsUser User { get; set; }

        public List<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}
