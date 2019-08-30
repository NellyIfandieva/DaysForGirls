﻿using System.Collections.Generic;

namespace DaysForGirls.Web.ViewModels
{
    public class ManufacturerDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Logo { get; set; }

        public List<ProductOfManufacturerViewModel> Products { get; set; }
    }
}
