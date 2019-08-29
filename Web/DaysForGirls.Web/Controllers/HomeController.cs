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
            await Task.Delay(0);
            return Redirect("/Sales/All");
        }

        [HttpPost]
        public async Task<IActionResult> Display(string criteria)
        {
            return Redirect("/Search/Display/" + criteria);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            await Task.Delay(0);
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Privacy()
        {
            await Task.Delay(0);
            return View();
        }
    }
}
