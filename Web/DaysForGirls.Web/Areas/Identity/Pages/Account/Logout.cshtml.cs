using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DaysForGirls.Data.Models;
using DaysForGirls.Data;
using Microsoft.EntityFrameworkCore;

namespace DaysForGirls.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<DaysForGirlsUser> signInManager;
        private readonly DaysForGirlsDbContext db;

        public LogoutModel(
            SignInManager<DaysForGirlsUser> signInManager,
            DaysForGirlsDbContext db)
        {
            this.signInManager = signInManager;
            this.db = db;
        }

        public async Task<IActionResult> OnGet()
        {
            string userId = this.signInManager.UserManager.GetUserId(this.User);

            var userShoppingCart = await this.db.ShoppingCarts
                .SingleOrDefaultAsync(sC => sC.UserId == userId);

            if(userShoppingCart != null)
            {
                this.db.ShoppingCarts.Remove(userShoppingCart);
                await this.db.SaveChangesAsync();
            }

            await this.signInManager.SignOutAsync();

            return Redirect("/");
            //return Redirect("/Identity/Account/Login");
        }
    }
}