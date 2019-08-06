using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Web.ViewModels
{
    public class ProductInSaleViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<PictureDisplayAllViewModel> Pictures { get; set; }

        public decimal OldPrice { get; set; }

        public decimal NewPrice => this.OldPrice - (0.3M * this.OldPrice);

        public int Quantity { get; set; }
    }
}
