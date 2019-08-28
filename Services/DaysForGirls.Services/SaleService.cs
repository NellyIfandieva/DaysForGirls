namespace DaysForGirls.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DaysForGirls.Data;
    using DaysForGirls.Data.Models;
    using DaysForGirls.Services.Models;
    using Microsoft.EntityFrameworkCore;

    public class SaleService : ISaleService
    {
        private readonly DaysForGirlsDbContext db;

        public SaleService(DaysForGirlsDbContext db)
        {
            this.db = db;
        }

        public async Task<string> CreateAsync(SaleServiceModel saleServiceModel)
        {
            Sale sale = new Sale
            {
                Title = saleServiceModel.Title,
                EndsOn = saleServiceModel.EndsOn,
                Picture = saleServiceModel.Picture
            };

            this.db.Sales.Add(sale);
            int result = await db.SaveChangesAsync();
            string saleId = null;

            if(result > 0)
            {
                saleId = sale.Id;
            }

            return saleId;
        }

        public IQueryable<SaleServiceModel> DisplayAll()
        {
            var allSales = this.db.Sales
                .Where(s => s.IsDeleted == false
                && s.IsActive == true)
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
                .Where(s => s.IsDeleted == false)
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

        public async Task<SaleServiceModel> GetSaleByIdAsync(string id)
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

        public async Task<SaleServiceModel> GetSaleByTitleAsync(string saleTitle)
        {
            var saleWithDetails = await this.db.Sales
                .Include(s => s.Products)
                .SingleOrDefaultAsync(sale => sale.Title == saleTitle);

            var productsInSale = await this.db.Products
                .Where(p => p.Sale.Title == saleWithDetails.Title)
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

        public async Task<bool> AddProductToSaleAsync(string saleId, int productId)
        {
            Sale sale = await this.db.Sales
                .SingleOrDefaultAsync(s => s.Id == saleId);

            Product productToAdd = await this.db.Products
                .SingleOrDefaultAsync(p => p.Id == productId);

            if(sale == null || productToAdd == null)
            {
                if(sale == null)
                {
                    throw new ArgumentNullException(nameof(sale));
                }
                else
                {
                    throw new ArgumentNullException(nameof(productToAdd));
                }
            }

            sale.Products.Add(productToAdd);

            this.db.Sales.Update(sale);

            int result = await this.db.SaveChangesAsync();

            bool productAddedToSale = result > 0;

            return productAddedToSale;
        }

        //TODO implement Edit

        public async Task<bool> EditAsync(SaleServiceModel model)
        {
            var saleToEdit = await this.db.Sales
                .SingleOrDefaultAsync(s => s.Id == model.Id);

            if(saleToEdit == null)
            {
                throw new ArgumentNullException(nameof(saleToEdit));
            }

            saleToEdit.Title = model.Title;
            saleToEdit.EndsOn = model.EndsOn;
            saleToEdit.Picture = model.Picture;

            this.db.Update(saleToEdit);
            int result = await this.db.SaveChangesAsync();

            bool saleIsEdited = result > 0;

            return saleIsEdited;
        }

        public async Task<bool> DeleteSaleById(string saleId)
        {
            Sale saleToDelete = await this.db.Sales
                .Include(s => s.Products)
                .SingleOrDefaultAsync(s => s.Id == saleId);

            HashSet<Product> productsOutOfSale = saleToDelete.Products.ToHashSet();
            

            foreach(var product in productsOutOfSale)
            {
                product.IsInSale = false;
            }

            this.db.UpdateRange(productsOutOfSale);
            saleToDelete.IsDeleted = true;
            this.db.Update(saleToDelete);
            int result = await this.db.SaveChangesAsync();

            bool saleIsDeleted = result > 0;

            return saleIsDeleted;
        }
    }
}
