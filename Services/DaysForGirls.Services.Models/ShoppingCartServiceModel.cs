using System.Collections.Generic;

namespace DaysForGirls.Services.Models
{
    public class ShoppingCartServiceModel
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public List<ShoppingCartItemServiceModel> ShoppingCartItems { get; set; }
    }
}
