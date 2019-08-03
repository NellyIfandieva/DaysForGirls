using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DaysForGirls.Data.Models;

namespace DaysForGirls.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<DaysForGirlsUser> signInManager;

        public LogoutModel(SignInManager<DaysForGirlsUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        public async Task<IActionResult> OnGet()
        {
            await this.signInManager.SignOutAsync();
            return Redirect("/Home/Index");
            //return Redirect("/Identity/Account/Login");
        }
    }
}