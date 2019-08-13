using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Web.ViewModels
{
    public class DisplayAllOfCategoryViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Picture { get; set; }

        public string Price { get; set; }

        public bool IsInSale { get; set; }

        public string SaleId { get; set; }
    }
}
