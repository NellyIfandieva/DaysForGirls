﻿using System.Collections.Generic;
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
                    IsActive = s.IsActive,
                    Products = s.Products
                        .Where(sI => sI.SaleId == s.Id)
                        .Select(pr => new ProductSaleServiceModel
                        {
                            Id = pr.Id,
                            Product = new ProductServiceModel
                            {
                                Id = pr.Product.Id
                            }
                        }).ToList()
                });

            return allSales;
        }

        public async Task<SaleServiceModel> GetSaleByIdAsync(int id)
        {
            var saleWithDetails = await this.db.Sales
                .SingleOrDefaultAsync(sale => sale.Id == id);

            var productsInSale = this.db.ProductsSales
                .Where(p => p.SaleId == id)
                .Select(pS => new ProductSaleServiceModel
                {
                    Id = pS.Id,
                    Product = new ProductServiceModel
                    {
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
                            .Select(p => new PictureServiceModel
                            {
                                Id = p.Id,
                                PictureUrl = p.PictureUrl
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
                        },
                        Reviews = pS.Product.Reviews
                            .Select(pR => new CustomerReviewServiceModel
                            {
                                Id = pR.Id,
                                Title = pR.Title,
                                Text = pR.Text,
                                CreatedOn = pR.CreatedOn.ToString("dddd dd MMMM yyyy"),
                                AuthorUsername = pR.Author.UserName
                            }).ToList()
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
            
            return saleToReturn;
        }

        public async Task<bool> AddProductToSale(SaleServiceModel saleToAddTo)
        {
            Sale sale = this.db.Sales
                .SingleOrDefault(s => s.Id == saleToAddTo.Id);

            ProductSale productToAdd = new ProductSale
            {
                ProductId = saleToAddTo.NewProduct.ProductId,
                SaleId = sale.Id
            };

            sale.Products.Add(productToAdd);

            this.db.Sales.Update(sale);

            int result = await this.db.SaveChangesAsync();

            bool productAddedToSale = result > 0;

            return productAddedToSale;
        }
    }
}
