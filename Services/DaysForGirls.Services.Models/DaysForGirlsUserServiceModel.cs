using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DaysForGirls.Services.Models
{
    public class DaysForGirlsUserServiceModel : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Address { get; set; }

        public List<OrderServiceModel> Orders { get; set; }

        public List<CustomerReviewServiceModel> ProductReviews { get; set; }

        public bool IsDeleted { get; set; }
    }
}
