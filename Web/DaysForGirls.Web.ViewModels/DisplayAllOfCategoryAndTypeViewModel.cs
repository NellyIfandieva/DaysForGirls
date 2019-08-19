using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Web.ViewModels
{
    public class DisplayAllOfCategoryAndTypeViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Picture { get; set; }

        public string Price { get; set; }

        public int AvailableItems { get; set; }

        public string SaleId { get; set; }

        public string ShoppingCartId { get; set; }
    }
}
