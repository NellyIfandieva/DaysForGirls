using System.Collections.Generic;

namespace DaysForGirls.Web.ViewModels
{
    public class SaleDisplayAllViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string EndsOn { get; set; }

        public string Picture { get; set; }

        public List<ProductInSaleViewModel> Products { get; set; }
    }
}
