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

        public string CreatedOn { get; set; }

        public string AuthorUsername { get; set; }

        public int ProductId { get; set; }

        public bool IsDeleted { get; set; }
    }
}
