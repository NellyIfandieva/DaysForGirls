using Microsoft.AspNetCore.Identity;

namespace DaysForGirls.Services.Models
{
    public class DaysForGirlsUserServiceModel : IdentityUser
    {
        public override string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
