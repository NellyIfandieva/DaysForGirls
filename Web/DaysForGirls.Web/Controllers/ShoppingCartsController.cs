using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DaysForGirls.Services;
using DaysForGirls.Services.Models;
using DaysForGirls.Web.InputModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DaysForGirls.Web.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly IShoppingCartService shoppingCartService;
        private readonly IProductService productService;

        public ShoppingCartsController(IShoppingCartService shoppingCartService,
            IProductService productService)
        {
            this.shoppingCartService = shoppingCartService;
            this.productService = productService;
        }

        public async Task<IActionResult> AddProduct(int productId)
        {
            if(this.User.Identity.IsAuthenticated == false)
            {
                return Redirect("Areas/Identity/Account/Login");
            }
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var product = await this.productService.GetProductDetailsById(productId);

            ShoppingCartItemServiceModel item = new ShoppingCartItemServiceModel
            {
                Product = product,
                Quantity = 1
            };

            bool cartIsCreated = await this.shoppingCartService.CreateCart(userId, item);

            bool productQuantityIsUpdated = await this.productService.UpdateProductQuantity(product.Id);

            return Redirect("/Products/Details/{productId}");
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
