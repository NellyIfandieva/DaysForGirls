using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaysForGirls.Services;
using DaysForGirls.Services.Models;
using DaysForGirls.Web.InputModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DaysForGirls.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService shoppingCartService;
        private readonly IProductService productService;

        public ShoppingCartController(IShoppingCartService shoppingCartService,
            IProductService productService)
        {
            this.shoppingCartService = shoppingCartService;
            this.productService = productService;
        }

        public async Task<bool> Add(ShoppingCartInputModel model)
        {
            var productToAdd = this.productService.GetDetailsOfProductByIdAsync(model.ProductId);

            if (productToAdd == null)
            {
                //
            }

            ProductServiceModel productServiceModel = new ProductServiceModel
            {
                Id = productToAdd.Id
            };

            ShoppingCartItemServiceModel itemServiceModel = new ShoppingCartItemServiceModel
            {
                Product = productServiceModel,
                Quantity = model.Quantity
            };

            //ShoppingCartItem itemToAdd = this.shoppingCartService.Create(itemServiceModel);

            bool isAdded = await this.shoppingCartService.Add(itemServiceModel);

            return isAdded;
        }
    }
}
