using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DaysForGirls.Data;
using DaysForGirls.Services;
using DaysForGirls.Services.Models;
using DaysForGirls.Web.InputModels;
using DaysForGirls.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DaysForGirls.Web.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly DaysForGirlsDbContext db;
        private readonly IShoppingCartService shoppingCartService;
        private readonly IAdminService adminService;
        private readonly IProductService productService;

        public ShoppingCartsController(
            DaysForGirlsDbContext db,
            IShoppingCartService shoppingCartService,
            IAdminService adminService,
            IProductService productService)
        {
            this.db = db;
            this.shoppingCartService = shoppingCartService;
            this.adminService = adminService;
            this.productService = productService;
        }


        [HttpGet("/ShoppingCarts/AddProduct/{productId}")]
        public async Task<IActionResult> AddProduct(int productId)
        {
            if(this.User.Identity.IsAuthenticated == false)
            {
                return Redirect("Areas/Identity/Account/Login");
            }
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var product = await this.productService.GetProductByIdAsync(productId);

            ShoppingCartItemServiceModel item = new ShoppingCartItemServiceModel
            {
                Product = product,
                Quantity = 1
            };

            string cartId = await this.shoppingCartService.CreateCart(userId, item);

            bool productIsAdded = await this.productService.AddProductToShoppingCart(product.Id, cartId);

            return Redirect("/Products/Details/{productId}");
        }

        [HttpGet("/ShoppingCarts/Display")]
        public async Task<IActionResult> Display()
        {
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var shoppingCart = this.db.ShoppingCarts
                .SingleOrDefault(sC => sC.UserId == userId);

            var itemsInCart = shoppingCart.ShoppingCartItems.ToList();

            return View();
        }
        //public async Task<bool> Add(ShoppingCartInputModel model)
        //{
        //    var productToAdd = this.productService.GetProductDetailsById(model.ProductId);

        //    if (productToAdd == null)
        //    {
        //        //
        //    }

        //    ProductServiceModel productServiceModel = new ProductServiceModel
        //    {
        //        Id = productToAdd.Id
        //    };

        //    ShoppingCartItemServiceModel itemServiceModel = new ShoppingCartItemServiceModel
        //    {
        //        Product = productServiceModel,
        //        Quantity = model.Quantity
        //    };

        //    //ShoppingCartItem itemToAdd = this.shoppingCartService.Create(itemServiceModel);

        //    bool isAdded = await this.shoppingCartService.Add(itemServiceModel);

        //    return isAdded;
        //}
    }
}
