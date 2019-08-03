using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DaysForGirls.Data.Models
{
    public class DaysForGirlsUser : IdentityUser
    {
        public DaysForGirlsUser()
        {
            this.ShoppingCarts = new HashSet<ShoppingCart>();
            this.Orders = new HashSet<Order>();
            this.ProductReviews = new HashSet<CustomerReview>();
        }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Address { get; set; }

        public string FullName => this.FirstName + " " + this.LastName;

        public ICollection<ShoppingCart> ShoppingCarts { get; set; }

        public ICollection<Order> Orders { get; set; }

        public ICollection<CustomerReview> ProductReviews { get; set; }

        public bool IsDeleted { get; set; }
    }
}
