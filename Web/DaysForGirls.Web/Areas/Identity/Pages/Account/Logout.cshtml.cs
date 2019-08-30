using DaysForGirls.Data;
using DaysForGirls.Data.Models;
using DaysForGirls.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DaysForGirls.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<DaysForGirlsUser> signInManager;
        private readonly DaysForGirlsDbContext db;
        private readonly IAdminService adminService;

        public LogoutModel(
            SignInManager<DaysForGirlsUser> signInManager,
            DaysForGirlsDbContext db,
            IAdminService adminService)
        {
            this.signInManager = signInManager;
            this.db = db;
            this.adminService = adminService;
        }

        public async Task<IActionResult> OnGet()
        {
            string userId = this.signInManager.UserManager.GetUserId(this.User);

            var userShoppingCart = await this.db.ShoppingCarts
                .Include(sC => sC.ShoppingCartItems)
                .SingleOrDefaultAsync(sC => sC.UserId == userId);

            if (userShoppingCart != null)
            {
                var allItemsInCart = userShoppingCart.ShoppingCartItems;

                List<int> productsInCartIds = new List<int>();

                foreach (var item in allItemsInCart)
                {
                    productsInCartIds.Add(item.ProductId);
                }

                this.db.ShoppingCartItems.RemoveRange(allItemsInCart);
                userShoppingCart.ShoppingCartItems.Clear();

                bool productsCartIdSetToNull = await this.adminService
                    .SetProductsCartIdToNullAsync(productsInCartIds);

                this.db.ShoppingCarts.Remove(userShoppingCart);
                await this.db.SaveChangesAsync();
            }

            await this.signInManager.SignOutAsync();

            return Redirect("/");
            //return Redirect("/Identity/Account/Login");
        }
    }
}