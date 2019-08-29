namespace DaysForGirls.Web.Controllers
{
    using Services;
    using ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductsController : Controller
    {
        private readonly IProductService productService;
        private readonly IAdminService adminService;

        public ProductsController(
            IProductService productService,
            IAdminService adminService)
        {
            this.productService = productService;
            this.adminService = adminService;
        }

        [HttpGet("/Products/All")]
        public async Task<IActionResult> All()
        {
            var allProducts = await this.productService
                .DisplayAll()
                .Select(p => new ProductDisplayAllViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price.ToString("f2"),
                    AvailableItems = p.AvailableItems,
                    Picture = p.Picture.PictureUrl,
                    SaleId = p.SaleId,
                    ShoppingCartId = p.ShoppingCartId,
                    OrderId = p.OrderId
                }).ToListAsync();

            return View(allProducts);
        }

        [HttpGet("/Products/Details/{productId}")]
        public async Task<IActionResult> Details(int productId)
        {
            if(productId <= 0)
            {
                return BadRequest();
            }

            var productFromDb = await this.adminService
                .GetProductByIdAsync(productId);

            var productToDisplay = new ProductDetailsGeneralUserViewModel
            {
                Id = productFromDb.Id,
                Name = productFromDb.Name,
                Description = productFromDb.Description,
                Colour = productFromDb.Colour,
                Size = productFromDb.Size,
                Price = productFromDb.Price.ToString("f2"),
                AvailableItems = productFromDb.Quantity.AvailableItems,
                Pictures = productFromDb.Pictures
                    .Select(pic => new PictureDetailsViewModel
                    {
                        Id = pic.Id,
                        PictureUrl = pic.PictureUrl
                    })
                    .ToList(),
                ManufacturerId = productFromDb.Manufacturer.Id,
                ManufacturerName = productFromDb.Manufacturer.Name,
                Reviews = productFromDb.Reviews
                    .Select(r => new CustomerReviewAllViewModel
                    {
                        Id = r.Id,
                        Title = r.Title,
                        Text = r.Text,
                        DateCreated = r.CreatedOn,
                        Author = r.AuthorUsername
                    })
                    .ToList(),
                SaleId = productFromDb.SaleId,
                ShoppingCartId = productFromDb.ShoppingCartId,
                OrderId = productFromDb.OrderId
            };

            return View(productToDisplay);
        }
    }
}