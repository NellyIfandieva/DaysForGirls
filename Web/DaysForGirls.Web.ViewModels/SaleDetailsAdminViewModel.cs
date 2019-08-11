using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Web.ViewModels
{
    public class SaleDetailsAdminViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string EndsOn { get; set; }

        public bool IsActive { get; set; }

        public List<ProductInSaleAdminViewModel> Products { get; set; }
    }
}
