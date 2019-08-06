using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaysForGirls.Data;
using DaysForGirls.Data.Models;
using DaysForGirls.Services.Models;

namespace DaysForGirls.Services
{
    public class SaleService : ISaleService
    {
        private readonly DaysForGirlsDbContext db;

        public SaleService(DaysForGirlsDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> Create(SaleServiceModel saleServiceModel)
        {
            Sale sale = new Sale
            {
                Title = saleServiceModel.Title,
                EndsOn = saleServiceModel.EndsOn,
                Picture = saleServiceModel.Picture
            };

            this.db.Sales.Add(sale);
            int result = await db.SaveChangesAsync();

            return result == 1;
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
                    Picture = ss.Picture
                });

            return allSales;
        }

        public IQueryable<SaleServiceModel> DisplayAllAdmin()
        {
            var allSales = this.db.Sales
                .Select(s => new SaleServiceModel
                {
                    Id = s.Id,
                    Title = s.Title,
                    EndsOn = s.EndsOn,
                    Picture = s.Picture,
                    IsActive = s.IsActive
                });

            return allSales;
        }

        public async Task<SaleServiceModel> GetSaleByIdAsync(int id)
        {
            var saleWithDetails = this.db.Sales
                .SingleOrDefault(sale => sale.Id == id);

            SaleServiceModel saleToReturn = new SaleServiceModel
            {
                Id = saleWithDetails.Id,
                Title = saleWithDetails.Title,
                EndsOn = saleWithDetails.EndsOn,
                Picture = saleWithDetails.Picture,
                Products = saleWithDetails.Products
                    .Select(product => new ProductServiceModel
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        Quantity = new QuantityServiceModel
                        {
                            AvailableItems = product.Quantity.AvailableItems
                        },
                        Pictures = product.Pictures
                            .Select(pp => new PictureServiceModel
                            {
                                PictureUrl = pp.PictureUrl
                            }).ToList()

                    }).ToList()
            };

            await Task.Delay(0);
            
            return saleToReturn;
        }

        public async Task<bool> AddProductToSale(int saleId, int productId)
        {
            Product product = this.db.Products
                .SingleOrDefault(p => p.Id == productId);

            Sale sale = this.db.Sales
                .SingleOrDefault(s => s.Id == saleId);
            sale.Products.Add(product);

            this.db.Update(sale);
            int result = await this.db.SaveChangesAsync();

            return result == 1;
        }
    }
}
