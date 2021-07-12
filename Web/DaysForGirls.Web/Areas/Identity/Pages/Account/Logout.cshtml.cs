namespace DaysForGirls.Web.Areas.Identity.Pages.Account
{
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using Services;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

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

            var userShoppingCarts = await this.db.ShoppingCarts
                .Include(sC => sC.ShoppingCartItems)
                .Where(sC => sC.UserId == userId).ToArrayAsync();

            if (userShoppingCarts.Length > 0)
            {
                foreach(var cart in userShoppingCarts)
                {
                    var allItemsInCart = cart.ShoppingCartItems;

                    var productsInCartIds = new List<int>();

                    foreach (var item in allItemsInCart)
                    {
                        productsInCartIds.Add(item.ProductId);
                    }

                    this.db.ShoppingCartItems.RemoveRange(allItemsInCart);
                    cart.ShoppingCartItems.Clear();

                    int? productsCartIdSetToNull = null;

                    while(productsCartIdSetToNull == null)
                    {
                        productsCartIdSetToNull = await this.adminService
                        .SetProductsCartIdToNullAsync(productsInCartIds);

                        if(productsCartIdSetToNull != null)
                        {
                            break;
                        }
                    }
                }
                this.db.ShoppingCarts.RemoveRange(userShoppingCarts);
                await this.db.SaveChangesAsync();
            }

            await this.signInManager.SignOutAsync();

            return Redirect("/");
            //return Redirect("/Identity/Account/Login");
        }
    }
}