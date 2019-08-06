using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Web.ViewModels
{
    public class SaleDetailsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime EndsOn { get; set; }

        public bool IsValid { get; set; }

        public List<ProductInSaleViewModel> Products { get; set; }
    }
}
