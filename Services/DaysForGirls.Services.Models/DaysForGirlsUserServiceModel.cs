using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DaysForGirls.Services.Models
{
    public class DaysForGirlsUserServiceModel : IdentityUser
    {
        public string Id { get; set; }
    }
}
