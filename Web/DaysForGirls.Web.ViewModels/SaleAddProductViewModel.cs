using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Web.ViewModels
{
    public class SaleAddProductViewModel
    {
        public SaleAddProductViewModel()
        {
            this.ProductIds = new List<int>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public string Picture { get; set; }

        public string Price { get; set; }

        public ICollection<int> ProductIds { get; set; }
    }
}
