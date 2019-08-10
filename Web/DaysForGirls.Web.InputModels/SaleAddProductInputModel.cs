using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Web.InputModels
{
    public class SaleAddProductInputModel
    {
        public string Id { get; set; }

        public string ProductName { get; set; }

        public string Picture { get; set; }

        public decimal Price { get; set; }

        public int SaleId { get; set; }
    }
}
