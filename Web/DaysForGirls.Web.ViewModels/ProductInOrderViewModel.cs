﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Web.ViewModels
{
    public class ProductInOrderViewModel
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductColour { get; set; }

        public string ProductSize { get; set; }

        public string ProductPicture { get; set; }

        public decimal ProductPrice { get; set; }

        public int ProductQuantity { get; set; }
    }
}
