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

            var productsInSale = this.db.ProductsSales
                .Where(p => p.SaleId == id)
                .Select(pS => new ProductServiceModel
                {
                    Id = pS.ProductId,
                    Name = pS.Product.Name,
                    Category = new CategoryServiceModel
                    {
                        Name = pS.Product.Category.Name
                    },
                    ProductType = new ProductTypeServiceModel
                    {
                        Name = pS.Product.ProductType.Name
                    },
                    Pictures = pS.Product.Pictures
                        .Select(pU => new PictureServiceModel
                        {
                            Id = pU.Id,
                            PictureUrl = pU.PictureUrl,
                            ProductId = pU.Product.Id
                        }).ToList(),
                    Description = pS.Product.Description,
                    Colour = pS.Product.Colour,
                    Size = pS.Product.Size,
                    Price = pS.Product.Price,
                    Manufacturer = new ManufacturerServiceModel
                    {
                        Name = pS.Product.Manufacturer.Name
                    },
                    Quantity = new QuantityServiceModel
                    {
                        AvailableItems = pS.Product.Quantity.AvailableItems
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

            ProductSale productSale = new ProductSale();
            productSale.Product = product;
            productSale.Sale = sale;

            sale.Products.Add(productSale);

            product.Sales.Add(productSale);
            product.IsInSale = true;

            this.db.ProductsSales.Add(productSale);
            this.db.Sales.Update(sale);
            this.db.Products.Update(product);

            int result = await this.db.SaveChangesAsync();

            bool productAddedToSale = result > 0;
            return productAddedToSale;
        }
    }
}
