using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DaysForGirls.Services.Models
{
    public class DaysForGirlsUserServiceModel : IdentityUser
    {
        public override string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
