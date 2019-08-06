using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaysForGirls.Services;
using DaysForGirls.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DaysForGirls.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly IProductService productService;

        protected BaseController(IProductService productService)
        {
            this.productService = productService;
        }
    }
}