using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Services.Models
{
    public class DisplayAllOfCategoryProductServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public PictureServiceModel Picture { get; set; }

        public decimal Price { get; set; }
    }
}
