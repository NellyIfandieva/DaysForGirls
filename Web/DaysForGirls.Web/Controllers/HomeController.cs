using DaysForGirls.Services;
using DaysForGirls.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DaysForGirls.Web.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public IActionResult Index()
        {
            return Redirect("/Sales/All");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Display(string criteria)
        {
            if(criteria == null)
            {
                return Redirect("/Home/Error");
            }

            await Task.Delay(0);
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
