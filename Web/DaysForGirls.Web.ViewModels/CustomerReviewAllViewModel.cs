using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Web.ViewModels
{
    public class CustomerReviewAllViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public string DateCreated { get; set; }

        //public string AuthorId { get; set; }

        public string AuthorUsername { get; set; }
    }
}
