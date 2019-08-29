namespace DaysForGirls.Web.Controllers
{
    using Services;
    using ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    public class SalesController : Controller
    {
        private readonly ISaleService saleService;

        public SalesController(ISaleService saleService)
        {
            this.saleService = saleService;
        }

        [HttpGet("/Sales/All")]
        public async Task<IActionResult> All()
        {
            var allSales = await this.saleService
                .DisplayAll()
                .Select(sale => new SaleDisplayAllViewModel
                {
                    Id = sale.Id,
                    Title = sale.Title,
                    Picture = sale.Picture,
                    EndsOn = sale.EndsOn.ToString("dddd, dd MMMM yyyy")
                })
                .ToListAsync();

            return View(allSales);
        }

        [HttpGet("/Sales/Details/{id}")]
        public async Task<IActionResult> Details(string saleId)
        {
            //if(saleId == null)
            //{
            //    return BadRequest();
            //}

            var sale = await this.saleService.GetSaleByIdAsync(saleId);

            var saleToDisplay = new SaleDetailsViewModel
            {
                Id = sale.Id,
                Title = sale.Title,
                Picture = sale.Picture,
                EndsOn = sale.EndsOn.DayOfWeek
                    + ", "
                    + sale.EndsOn.Day
                    + "-"
                    + sale.EndsOn.Month
                    + "-"
                    + sale.EndsOn.Year,
                Products = sale.Products
                    .Select(p => new ProductInSaleViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        MainPicture = p.Pictures.ElementAt(0).PictureUrl,
                        OldPrice = p.Price,
                        AvailableItems = p.Quantity.AvailableItems,
                        ShoppingCartId = p.ShoppingCartId,
                        OrderId = p.OrderId
                    }).ToList()
            };

            return View(saleToDisplay);
        }
    }
}
