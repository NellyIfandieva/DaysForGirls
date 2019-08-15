using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Web.ViewModels
{
    public class SaleDetailsViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string EndsOn { get; set; }

        public string Picture { get; set; }

        public List<ProductInSaleViewModel> Products { get; set; }
    }
}
