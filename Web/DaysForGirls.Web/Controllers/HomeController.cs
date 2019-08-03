using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DaysForGirls.Web.Models;
using DaysForGirls.Services;
using Microsoft.EntityFrameworkCore;

namespace DaysForGirls.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISaleService salesService;

        public HomeController(ISaleService salesService)
        {
            this.salesService = salesService;
        }

        [HttpGet("/")]
        public async Task<IActionResult> Index()
        {
            return Redirect("/Sales/All");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
