using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DaysForGirls.Data.Models
{
    public class CustomerReview : BaseModel<int>
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string AuthorId { get; set; }
        public DaysForGirlsUser Author { get; set; }

        public bool IsDeleted => this.Product.IsDeleted == true || this.Author.IsDeleted == true;
    }
}
