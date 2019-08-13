using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaysForGirls.Data;
using DaysForGirls.Data.Models;
using DaysForGirls.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace DaysForGirls.Services
{
    public class SaleService : ISaleService
    {
        private readonly DaysForGirlsDbContext db;

        public SaleService(DaysForGirlsDbContext db)
        {
            this.db = db;
        }

        public async Task<int> Create(SaleServiceModel saleServiceModel)
        {
            Sale sale = new Sale
            {
                Title = saleServiceModel.Title,
                EndsOn = saleServiceModel.EndsOn,
                Picture = saleServiceModel.Picture
            };

            this.db.Sales.Add(sale);
            int result = await db.SaveChangesAsync();
            int saleId = 0;

            if(result > 0)
            {
                saleId = sale.Id;
            }

            return saleId;
        }

        public IQueryable<SaleServiceModel> DisplayAll()
        {
            var allSales = this.db.Sales
                .Where(s => s.IsActive == true)
                .Select(ss => new SaleServiceModel
                {
                    Id = ss.Id,
                    Title = ss.Title,
                    EndsOn = ss.EndsOn,
                    Picture = ss.Picture,
                    Products = ss.Products
                        .Select(p => new ProductServiceModel
                        {
                            Id = p.Id,
                            Name = p.Name
                        })
                        .ToList()
                });

            return allSales;
        }

        public IQueryable<SaleServiceModel> DisplayAllAdmin()
        {
            var allSales = this.db.Sales
                .OrderBy(s => s.EndsOn)
                .Include(s => s.Products)
                .Select(s => new SaleServiceModel
                {
                    Id = s.Id,
                    Title = s.Title,
                    EndsOn = s.EndsOn,
                    Picture = s.Picture,
                    IsActive = s.IsActive,
                    Products = s.Products
                    .Where(p => p.SaleId == s.Id)
                    .Select(p => new ProductServiceModel
                    {
                        Id = p.Id
                    })
                    .ToList()
                });

            return allSales;
        }

        public async Task<SaleServiceModel> GetSaleByIdAsync(int id)
        {
            var saleWithDetails = await this.db.Sales
                .Include(s => s.Products)
                .SingleOrDefaultAsync(sale => sale.Id == id);

            var productsInSale = await this.db.Products
                .Where(p => p.SaleId == id)
                .Select(pS => new ProductServiceModel
                {
                    Id = pS.Id,
                    Name = pS.Name,
                    Category = new CategoryServiceModel
                    {
                        Name = pS.Category.Name
                    },
                    ProductType = new ProductTypeServiceModel
                    {
                        Name = pS.ProductType.Name
                    },
                    Pictures = pS.Pictures
                        .Select(p => new PictureServiceModel
                        {
                            Id = p.Id,
                            PictureUrl = p.PictureUrl
                        }).ToList(),
                    Description = pS.Description,
                    Colour = pS.Colour,
                    Size = pS.Size,
                    Price = pS.Price,
                    Manufacturer = new ManufacturerServiceModel
                    {
                        Name = pS.Manufacturer.Name
                    },
                    Quantity = new QuantityServiceModel
                    {
                        AvailableItems = pS.Quantity.AvailableItems
                    },
                    Reviews = pS.Reviews
                        .Select(pR => new CustomerReviewServiceModel
                        {
                            Id = pR.Id,
                            Title = pR.Title,
                            Text = pR.Text,
                            CreatedOn = pR.CreatedOn.ToString("dddd dd MMMM yyyy"),
                            AuthorUsername = pR.Author.UserName
                        }).ToList()
                })
                .ToListAsync();

            SaleServiceModel saleToReturn = new SaleServiceModel
            {
                Id = saleWithDetails.Id,
                Title = saleWithDetails.Title,
                EndsOn = saleWithDetails.EndsOn,
                Picture = saleWithDetails.Picture,
                Products = productsInSale
            };
            
            return saleToReturn;
        }

        public async Task<bool> AddProductToSaleAsync(int saleId, int productId)
        {
            Sale sale = await this.db.Sales
                .SingleOrDefaultAsync(s => s.Id == saleId);

            Product toAdd = await this.db.Products
                .SingleOrDefaultAsync(p => p.Id == productId);

            sale.Products.Add(toAdd);

            this.db.Sales.Update(sale);

            int result = await this.db.SaveChangesAsync();

            bool productAddedToSale = result > 0;

            return productAddedToSale;
        }
    }
}
