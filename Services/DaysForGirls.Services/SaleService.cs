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

            var productsInSale = this.db.Products
                .Where(p => p.SaleId == id)
                .Select(p => new ProductServiceModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Category = new CategoryServiceModel
                    {
                        Name = p.Category.Name
                    },
                    ProductType = new ProductTypeServiceModel
                    {
                        Name = p.ProductType.Name
                    },
                    Pictures = p.Pictures
                        .Select(pU => new PictureServiceModel
                        {
                            Id = pU.Id,
                            PictureUrl = pU.PictureUrl,
                            ProductId = p.Id
                        }).ToList(),
                    Description = p.Description,
                    Colour = p.Colour,
                    Size = p.Size,
                    Price = p.Price,
                    Manufacturer = new ManufacturerServiceModel
                    {
                        Name = p.Manufacturer.Name
                    },
                    Quantity = new QuantityServiceModel
                    {
                        AvailableItems = p.Quantity.AvailableItems
                    }
                })
                .ToList();

            SaleServiceModel saleToReturn = new SaleServiceModel
            {
                Id = saleWithDetails.Id,
                Title = saleWithDetails.Title,
                EndsOn = saleWithDetails.EndsOn,
                Picture = saleWithDetails.Picture,
                Products = productsInSale
            };

            await Task.Delay(0);
            
            return saleToReturn;
        }

        public async Task<bool> AddProductToSale(int saleId, int productId)
        {
            Product product = this.db.Products
                .SingleOrDefault(p => p.Id == productId);

            var productPictures = this.db.Pictures
                .Where(pI => pI.ProductId == productId).ToList();

            product.Pictures = productPictures;

            Sale sale = this.db.Sales
                .SingleOrDefault(s => s.Id == saleId);
            sale.Products.Add(product);

            this.db.Update(sale);
            int result = await this.db.SaveChangesAsync();

            return result == 1;
        }
    }
}
