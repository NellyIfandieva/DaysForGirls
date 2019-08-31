using DaysForGirls.Data;
using DaysForGirls.Data.Models;
using DaysForGirls.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

            var userShoppingCarts = await this.db.ShoppingCarts
                .Include(sC => sC.ShoppingCartItems)
                .Where(sC => sC.UserId == userId).ToArrayAsync();

            if (userShoppingCarts.Count() > 0)
            {
                foreach(var cart in userShoppingCarts)
                {
                    var allItemsInCart = cart.ShoppingCartItems;

                    List<int> productsInCartIds = new List<int>();

                    foreach (var item in allItemsInCart)
                    {
                        productsInCartIds.Add(item.ProductId);
                    }

                    this.db.ShoppingCartItems.RemoveRange(allItemsInCart);
                    cart.ShoppingCartItems.Clear();

                    bool productsCartIdSetToNull = false;

                    while(productsCartIdSetToNull == false)
                    {
                        productsCartIdSetToNull = await this.adminService
                        .SetProductsCartIdToNullAsync(productsInCartIds);

                        if(productsCartIdSetToNull)
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