namespace DaysForGirls.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using System.Linq;
    using System.Threading.Tasks;
    using ViewModels;

    public class ManufacturersController : Controller
    {
        private readonly IManufacturerService manufacturerService;

        public ManufacturersController(IManufacturerService manufacturerService)
        {
            this.manufacturerService = manufacturerService;
        }

        [HttpGet("/Manufacturers/Details/{manufacturerId}")]
        public async Task<IActionResult> Details(int manufacturerId)
        {
            if (manufacturerId <= 0)
            {
                return Redirect("/Home/Error");
            }

            var manufacturer = await this.manufacturerService
                .GetManufacturerByIdAsync(manufacturerId);

            if(manufacturer == null)
            {
                return Redirect("/Home/Error");
            }

            var manufacturerToReturn = new ManufacturerDetailsViewModel
            {
                Id = manufacturer.Id,
                Name = manufacturer.Name,
                Description = manufacturer.Description,
                Logo = manufacturer.Logo.LogoUrl,
                Products = manufacturer.Products
                    .Select(p => new ProductOfManufacturerViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Picture = p.Pictures.ElementAt(0).PictureUrl,
                        Price = p.Price,
                        SalePrice = p.SalePrice,
                        AvailableItems = p.Quantity.AvailableItems,
                        SaleId = p.SaleId,
                        ShoppingCartId = p.ShoppingCartId,
                        OrderId = p.OrderId
                    })
                    .ToList()
            };

            return View(manufacturerToReturn);
        }
    }
}