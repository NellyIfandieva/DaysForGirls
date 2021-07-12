namespace DaysForGirls.Services
{
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductService : IProductService
    {
        private readonly DaysForGirlsDbContext db;

        public ProductService(
            DaysForGirlsDbContext db)
        {
            this.db = db;
        }

        public async Task<ProductAsShoppingCartItem> GetProductByIdAsync(int productId)
        {
            if(productId <= 0)
            {
                return null;
            }

            var product = await this.db.Products
                .Include(p => p.Quantity)
                .Include(p => p.Pictures)
                .Include(p => p.Sale)
                .Include(p => p.ShoppingCart)
                .SingleOrDefaultAsync(p => p.Id == productId
                && p.IsDeleted == false);

            if (product == null)
            {
                return null;
            }

            var picture = product.Pictures.ElementAt(0).PictureUrl;

            return new ProductAsShoppingCartItem
            {
                Id = product.Id,
                Name = product.Name,
                Colour = product.Colour,
                Size = product.Size,
                Price = product.Price,
                SalePrice = product.SalePrice,
                MainPictureUrl = picture,
                AvailableItems = product.Quantity.AvailableItems,
                SaleId = product.SaleId,
                ShoppingCartId = product.ShoppingCartId
            };
        }

        public async Task<IEnumerable<ProductDisplayAllServiceModel>> DisplayAll()
        {
            var allProducts = await this.db.Products
                .Where(p => p.IsDeleted == false)
                .Select(p => new ProductDisplayAllServiceModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    SalePrice = p.SalePrice,
                    Picture = new PictureServiceModel
                    {
                        PictureUrl = p.Pictures.ElementAt(0).PictureUrl
                    },
                    AvailableItems = p.Quantity.AvailableItems,
                    SaleId = p.SaleId,
                    ShoppingCartId = p.ShoppingCartId,
                    OrderId = p.OrderId
                }).ToListAsync();

            return allProducts;
        }

        public async Task<IEnumerable<DisplayAllOfCategoryProductServiceModel>> GetAllProductsOfCategory(string categoryName)
        {
            if(categoryName == null)
            {
                return null;
            }

            var allProductsOfCategory = await this.db.Products
                .Where(p => p.Category.Name == categoryName &&
                            p.IsDeleted == false)
                .Select(p => new DisplayAllOfCategoryProductServiceModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    SalePrice = p.SalePrice,
                    Picture = new PictureServiceModel
                    {
                        PictureUrl = p.Pictures.ElementAt(0).PictureUrl
                    },
                    AvailableItems = p.Quantity.AvailableItems,
                    IsInSale = p.IsInSale,
                    SaleId = p.SaleId,
                    ShoppingCartId = p.ShoppingCartId,
                    OrderId = p.OrderId
                }).ToListAsync();

            return allProductsOfCategory;
        }

        public async Task<IEnumerable<DisplayAllOfCategoryAndTypeServiceModel>> 
            GetAllProductsOfTypeAndCategory(string productTypeName, string categoryName)
        {
            if(productTypeName == null || categoryName == null)
            {
                return null;
            }

            var allProductsOfCategoryAndType = await this.db.Products
                .Where(p => p.Category.Name == categoryName && 
                            p.ProductType.Name == productTypeName && 
                            p.IsDeleted == false)
                .Select(p => new DisplayAllOfCategoryAndTypeServiceModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Picture = new PictureServiceModel
                    {
                        PictureUrl = p.Pictures.ElementAt(0).PictureUrl
                    },
                    Price = p.Price,
                    SalePrice = p.SalePrice,
                    AvailableItems = p.Quantity.AvailableItems,
                    SaleId = p.SaleId,
                    ShoppingCartId = p.ShoppingCartId,
                    OrderId = p.OrderId
                }).ToListAsync();

            return allProductsOfCategoryAndType;
        }

        public async Task<int?> AddProductToShoppingCartAsync(int productId, string shoppingCartId)
        {
            if(productId <= 0 || shoppingCartId == null)
            {
                return null;
            }

            var product = await this.db.Products
                .SingleOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                return null;
            }

            product.ShoppingCartId = shoppingCartId;
            product.Quantity.AvailableItems--;

            this.db.Update(product);
            return await this.db.SaveChangesAsync();
        }

        public async Task<int?> RemoveProductFromShoppingCartAsync(int productId)
        {
            if(productId <= 0)
            {
                return null;
            }

            var product = await this.db.Products
                .Include(p => p.Quantity)
                .Include(p => p.ShoppingCart)
                .SingleOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                return null;
            }

            product.Quantity.AvailableItems++;
            product.ShoppingCartId = null;

            this.db.Update(product);
            return await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductServiceModel>> 
            GetAllSearchResultsByCriteria(string criteria)
        {
            if(criteria == null)
            {
                return null;
            }

            var criteriaIsDecimal = decimal.TryParse(criteria, out decimal priceCriteria);

            if (criteriaIsDecimal == false)
            {
                priceCriteria = 0.00m;
            }

            string criteriaToLower = criteria.ToLower();

            var allSearchResults = new List<ProductServiceModel>();

            var searchResultsBatchOne = await db.Products
                .Where(p => p.Name.ToLower().Contains(criteriaToLower)
                || p.Description.ToLower().Contains(criteriaToLower)
                || p.Manufacturer.Name.ToLower().Contains(criteriaToLower)
                || p.Colour.ToLower().Contains(criteriaToLower)
                || p.Size.ToLower().Contains(criteriaToLower)
                || p.Sale.Title.ToLower().Contains(criteriaToLower)
                || p.Category.Name.ToLower().Contains(criteriaToLower)
                || p.ProductType.Name.ToLower().Contains(criteriaToLower)
                || p.Price <= priceCriteria
                || p.SaleId != null && p.SalePrice <= priceCriteria)
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
                    Description = p.Description,
                    Pictures = p.Pictures
                        .Select(pic => new PictureServiceModel
                        {
                            Id = pic.Id,
                            PictureUrl = pic.PictureUrl
                        })
                        .ToList(),
                    Colour = p.Colour,
                    Size = p.Size,
                    Manufacturer = new ManufacturerServiceModel
                    {
                        Id = p.ManufacturerId,
                        Name = p.Manufacturer.Name
                    },
                    Price = p.Price,
                    SalePrice = p.SalePrice,
                    Quantity = new QuantityServiceModel
                    {
                        Id = p.QuantityId,
                        AvailableItems = p.Quantity.AvailableItems
                    },
                    SaleId = p.SaleId,
                    ShoppingCartId = p.ShoppingCartId,
                    OrderId = p.OrderId,
                    Reviews = p.Reviews
                        .Select(r => new CustomerReviewServiceModel
                        {
                            Id = r.Id,
                            Title = r.Title,
                            Text = r.Text,
                            CreatedOn = r.CreatedOn.ToString("dddd, dd MMMM yyyy"),
                            AuthorId = r.AuthorId
                        })
                        .ToList()
                })
                .ToListAsync();

            allSearchResults.AddRange(searchResultsBatchOne);

            var allProductsInDb = this.db.Products
                .Include(p => p.Category)
                .Include(p => p.ProductType)
                .Include(p => p.Pictures)
                .Include(p => p.Manufacturer)
                .Include(p => p.Reviews)
                .Include(p => p.Order)
                .Include(p => p.Sale)
                .Include(p => p.ShoppingCart)
                .Include(p => p.Quantity)
                .ToHashSet();

            foreach (var product in allProductsInDb)
            {
                foreach (var review in product.Reviews)
                {
                    if (review.Title.ToLower().Contains(criteriaToLower)
                        || review.Text.ToLower().Contains(criteriaToLower))
                    {
                        var anotherSearchResult = new ProductServiceModel
                        {
                            Id = product.Id,
                            Name = product.Name,
                            Category = new CategoryServiceModel
                            {
                                Name = product.Category.Name
                            },
                            ProductType = new ProductTypeServiceModel
                            {
                                Name = product.ProductType.Name
                            },
                            Description = product.Description,
                            Pictures = product.Pictures
                                .Select(pic => new PictureServiceModel
                                {
                                    Id = pic.Id,
                                    PictureUrl = pic.PictureUrl
                                })
                                .ToList(),
                            Colour = product.Colour,
                            Size = product.Size,
                            Manufacturer = new ManufacturerServiceModel
                            {
                                Id = product.ManufacturerId,
                                Name = product.Manufacturer.Name
                            },
                            Price = product.Price,
                            SalePrice = product.SalePrice,
                            Quantity = new QuantityServiceModel
                            {
                                Id = product.QuantityId,
                                AvailableItems = product.Quantity.AvailableItems
                            },
                            SaleId = product.SaleId,
                            ShoppingCartId = product.ShoppingCartId,
                            OrderId = product.OrderId,
                            Reviews = product.Reviews
                                .Select(r => new CustomerReviewServiceModel
                                {
                                    Id = r.Id,
                                    Title = r.Title,
                                    Text = r.Text,
                                    CreatedOn = r.CreatedOn.ToString("dddd, dd MMMM yyyy"),
                                    AuthorId = r.AuthorId
                                })
                                .ToList()
                        };

                        allSearchResults.Add(anotherSearchResult);
                    }
                }
            }

            return allSearchResults;
        }

        public async Task<decimal> CalculateProductPriceAsync(int productId)
        {
            if(productId <= 0)
            {
                return 0.00m;
            }

            var product = await this.db.Products
                .SingleOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                return 0.00m;
            }

            if (product.SaleId != null)
            {
                product.Price = product.SalePrice;

                this.db.Update(product);
                await this.db.SaveChangesAsync();
            }

            return product.Price;
        }
    }
}
