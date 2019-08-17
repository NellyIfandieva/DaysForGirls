﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Web.ViewModels
{
    public class ProductDetailsGeneralUserViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Colour { get; set; }

        public string Size { get; set; }

        public string Price { get; set; }

        public int AvailableItems { get; set; }

        public List<PictureDetailsViewModel> Pictures { get; set; }

        public int ManufacturerId { get; set; }

        public string ManufacturerName { get; set; }

        public List<CustomerReviewAllViewModel> Reviews { get; set; }
    }
}
