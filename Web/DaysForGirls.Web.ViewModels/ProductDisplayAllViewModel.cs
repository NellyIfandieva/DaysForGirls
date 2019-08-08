﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Web.ViewModels
{
    public class ProductDisplayAllViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public string ProductType { get; set; }

        public decimal Price { get; set; }

        public string Picture { get; set; }

        public List<string> Pictures { get; set; }

        public int Quantity { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsInSale { get; set; }
    }
}
