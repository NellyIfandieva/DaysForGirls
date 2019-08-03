using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Services.Models
{
    public class CustomerReviewServiceModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        //public ProductServiceModel Product { get; set; }

        public DaysForGirlsUserServiceModel Author { get; set; }
    }
}
