namespace DaysForGirls.Services
{
    using Data;
    using Data.Models;
    using Services.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class SaleService : ISaleService
    {
        private readonly DaysForGirlsDbContext db;

        public SaleService(DaysForGirlsDbContext db)
        {
            this.db = db;
        }

        public async Task<string> CreateAsync(SaleServiceModel saleServiceModel)
        {
            var sale = new Sale
            {
                Title = saleServiceModel.Title,
                EndsOn = saleServiceModel.EndsOn,
                Picture = saleServiceModel.Picture
            };

            this.db.Sales.Add(sale);
            int result = await db.SaveChangesAsync();

            string saleId = sale.Id;

            return saleId;
        }

        public async Task<IEnumerable<SaleServiceModel>> DisplayAll()
        {
            var allSales = await this.db
                .Sales
                .Where(s => s.IsDeleted == false)
                .Select(s => new SaleServiceModel
                {
                    Id = s.Id,
                    Title = s.Title,
                    EndsOn = s.EndsOn,
                    Picture = s.Picture,
                    Products = s.Products
                        .Select(p => new ProductServiceModel
                        {
                            Id = p.Id,
                            Name = p.Name
                        })
                        .ToList()
                }).ToListAsync();

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

        public async Task<SaleServiceModel> GetSaleByIdAsync(string saleId)
        {
            if(saleId == null)
            {
                return null;
            }

            var saleWithDetails = await this.db.Sales
                .Include(s => s.Products)
                .SingleOrDefaultAsync(sale => sale.Id == saleId);

            if (saleWithDetails == null)
            {
                return null;
            }

            var productsInSale = await this.db.Products
                .Where(p => p.SaleId == saleId)
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
                    SalePrice = pS.SalePrice,
                    SaleId = pS.SaleId,
                    ShoppingCartId = pS.ShoppingCartId,
                    OrderId = pS.OrderId,
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
                            AuthorId = pR.AuthorId
                        }).ToList()
                })
                .ToListAsync();

            var saleToReturn = new SaleServiceModel
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
            if(saleTitle == null)
            {
                return null;
            }

            var saleWithDetails = await this.db.Sales
                .Include(s => s.Products)
                .SingleOrDefaultAsync(sale => sale.Title == saleTitle);

            if (saleWithDetails == null)
            {
                return null;
            }

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
                            AuthorId = pR.AuthorId
                        }).ToList()
                })
                .ToListAsync();

            var saleToReturn = new SaleServiceModel
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
            if(saleId == null || productId <= 0)
            {
                return false;
            }

            Sale sale = await this.db.Sales
                .SingleOrDefaultAsync(s => s.Id == saleId);

            Product productToAdd = await this.db.Products
                .SingleOrDefaultAsync(p => p.Id == productId);

            sale.Products.Add(productToAdd);

            this.db.Sales.Update(sale);

            int result = await this.db.SaveChangesAsync();

            bool productAddedToSale = result > 0;

            return productAddedToSale;
        }

        public async Task<bool> EditAsync(SaleServiceModel model)
        {
            if(model == null || model.Id == null)
            {
                return false;
            }

            var saleToEdit = await this.db.Sales
                .SingleOrDefaultAsync(s => s.Id == model.Id);

            if (saleToEdit == null)
            {
                return false;
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
            if(saleId == null)
            {
                return false;
            }

            Sale saleToDelete = await this.db.Sales
                .Include(s => s.Products)
                .SingleOrDefaultAsync(s => s.Id == saleId);

            if (saleToDelete == null)
            {
                return false;
            }

            HashSet<Product> productsOutOfSale = saleToDelete.Products
                .ToHashSet();

            if (productsOutOfSale.Count > 0)
            {
                foreach (var product in productsOutOfSale)
                {
                    product.IsInSale = false;
                }

                this.db.UpdateRange(productsOutOfSale);
            }

            this.db.Sales.Remove(saleToDelete);
            int result = await this.db.SaveChangesAsync();

            bool saleIsDeleted = result > 0;

            return saleIsDeleted;
        }
    }
}
