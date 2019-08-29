﻿namespace DaysForGirls.Services
{
    using DaysForGirls.Data;
    using DaysForGirls.Data.Models;
    using DaysForGirls.Services.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
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
            Product product = await this.db.Products
                .Include(p => p.Quantity)
                .Include(p => p.Pictures)
                .Include(p => p.Sale)
                .Include(p => p.ShoppingCart)
                .SingleOrDefaultAsync(p => p.Id == productId 
                && p.IsDeleted == false);

            if(product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            var picture = product.Pictures.ElementAt(0).PictureUrl;

            var productToReturn = new ProductAsShoppingCartItem
            {
                Id = product.Id,
                Name = product.Name,
                Colour = product.Colour,
                Size = product.Size,
                Price = product.Price,
                MainPictureUrl = picture,
                AvailableItems = product.Quantity.AvailableItems,
                SaleId = product.SaleId,
                ShoppingCartId = product.ShoppingCartId
            };

            return productToReturn;
        }

        public IQueryable<ProductDisplayAllServiceModel> DisplayAll()
        {
            var allProducts = this.db.Products
                .Where(p => p.IsDeleted == false)
                .Select(p => new ProductDisplayAllServiceModel
                 {
                     Id = p.Id,
                     Name = p.Name,
                     Price = p.Price,
                     Picture = new PictureServiceModel
                     {
                        PictureUrl = p.Pictures.ElementAt(0).PictureUrl
                     },
                     AvailableItems = p.Quantity.AvailableItems,
                     SaleId = p.SaleId,
                     ShoppingCartId = p.ShoppingCartId,
                     OrderId = p.OrderId
                 });

            return allProducts;
        }

        public IQueryable<DisplayAllOfCategoryProductServiceModel> GetAllProductsOfCategory(string categoryName)
        {
            var allProductsOfCategory = this.db.Products
                .Where(p => p.Category.Name == categoryName
                && p.IsDeleted == false)
                .Select(p => new DisplayAllOfCategoryProductServiceModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Picture = new PictureServiceModel
                    {
                        PictureUrl = p.Pictures.ElementAt(0).PictureUrl
                    },
                    AvailableItems = p.Quantity.AvailableItems,
                    IsInSale = p.IsInSale,
                    SaleId = p.SaleId,
                    ShoppingCartId = p.ShoppingCartId,
                    OrderId = p.OrderId
                });

            return allProductsOfCategory;
        }

        public IQueryable<DisplayAllOfCategoryAndTypeServiceModel> GetAllProductsOfTypeAndCategory(string productType, string category)
        {
            var allProductsOfCategoryAndType = this.db.Products
                .Where(p => p.Category.Name == category
                && p.ProductType.Name == productType
                && p.IsDeleted == false)
                .Select(p => new DisplayAllOfCategoryAndTypeServiceModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Picture = new PictureServiceModel
                    {
                        PictureUrl = p.Pictures.ElementAt(0).PictureUrl
                    },
                    Price = p.Price,
                    AvailableItems = p.Quantity.AvailableItems,
                    SaleId = p.SaleId,
                    ShoppingCartId = p.ShoppingCartId,
                    OrderId = p.OrderId
                });

            return allProductsOfCategoryAndType;
        }

        public async Task<bool> DeletePictureWithUrl(string pictureUrl)
        {
            var pictureToDelete = this.db.Pictures
                .SingleOrDefault(pic => pic.PictureUrl == pictureUrl);

            var product = await this.db.Products
                .SingleOrDefaultAsync(p => p.Id == pictureToDelete.ProductId);

            if(pictureToDelete == null
                || product == null)
            {
                if(pictureToDelete == null)
                {
                    throw new ArgumentNullException(nameof(pictureToDelete));
                }
                else
                {
                    throw new ArgumentNullException(nameof(product));
                }
            }

            product.Pictures.Remove(pictureToDelete);

            pictureToDelete.IsDeleted = true;

            this.db.UpdateRange(pictureToDelete, product);

            int result = await this.db.SaveChangesAsync();
            bool pictureIsDeleted = result > 0;

            return pictureIsDeleted;
        }

        //public async Task<bool> UploadNewPictureToProduct(int productId, string imageUrl)
        //{
        //var productInDb = await this.db.Products.
        //    SingleOrDefaultAsync(p => p.Id == productId);

        //Picture newPicture = new Picture
        //{
        //    PictureUrl = imageUrl
        //};

        //productInDb.Pictures.Add(newPicture);
        //    this.db.Update(productInDb);
        //    int result = await this.db.SaveChangesAsync();

        //bool pictureIsAdded = result > 0;

        //    return pictureIsAdded;
        //}

        //public async Task<bool> AddProductToSale(int productId, int saleId)
        //{
        //    var productSale = this.db.ProductsSales
        //        .SingleOrDefault(pS => pS.ProductId == productId
        //        && pS.SaleId == saleId);

        //    var product = this.db.Products
        //        .SingleOrDefault(p => p.Id == productId);

        //    product.Sales.Add(productSale);
        //    product.IsInSale = true;

        //    this.db.Products.Update(product);
        //    int result = await this.db.SaveChangesAsync();
        //    bool productIsAddedToSale = result > 0;

        //    return productIsAddedToSale;
        //}

        public async Task<bool> AddProductToShoppingCartAsync(int productId, string shoppingCartId)
        {
            var product = await this.db.Products
                .SingleOrDefaultAsync(p => p.Id == productId);

            if(product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            product.ShoppingCartId = shoppingCartId;
            product.Quantity.AvailableItems--;

            this.db.Update(product);
            int result = await this.db.SaveChangesAsync();

            bool productQuantityIsUpdated = result > 0;

            return productQuantityIsUpdated;
        }

        public async Task<bool> RemoveProductFromShoppingCartAsync(int productId)
        {
            var product = await this.db.Products
                .Include(p => p.Quantity)
                .Include(p => p.ShoppingCart)
                .SingleOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            product.Quantity.AvailableItems++;
            product.ShoppingCartId = null;

            this.db.Update(product);
            int result = await this.db.SaveChangesAsync();

            bool productIsOutOfCart = result > 0;

            return productIsOutOfCart;
        }

        public IQueryable<ProductServiceModel> GetAllSearchResultsByCriteria(string criteria)
        {
            decimal priceCriteria;
            bool criteriaIsDecimal = decimal.TryParse(criteria, out priceCriteria);

            if(criteriaIsDecimal == false)
            {
                priceCriteria = 0.00m;
            }

            string criteriaToLower = criteria.ToLower();

            var allSearchResults = this.db.Products
                .Include(p => p.Category)
                .Include(p => p.ProductType)
                .Include(p => p.Pictures)
                .Include(p => p.Manufacturer)
                .Include(p => p.Reviews)
                .Where(p => p.Name.ToLower().Contains(criteriaToLower)
                || p.Description.ToLower().Contains(criteriaToLower)
                || p.Manufacturer.Name.ToLower().Contains(criteriaToLower)
                || p.Colour.ToLower().Contains(criteriaToLower)
                || p.Size.ToLower().Contains(criteriaToLower)
                || p.Sale.Title.ToLower().Contains(criteriaToLower)
                || p.Category.Name.ToLower().Contains(criteriaToLower)
                || p.ProductType.Name.ToLower().Contains(criteriaToLower)
                || p.Price <= priceCriteria)
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
                            AuthorUsername = r.Author.FullName
                        })
                        .ToList()
                });

            //IQueryable<ProductServiceModel> allSearchResults = null;

                //if(criteriaIsDecimal)
                //{
                //    allSearchResults = this.db.Products
                //        .Include(p => p.Category)
                //        .Include(p => p.ProductType)
                //        .Include(p => p.Pictures)
                //        .Include(p => p.Manufacturer)
                //        .Include(p => p.Reviews)
                //        .Where(p => p.Price <= priceCriteria
                //        && p.IsDeleted == false)
                //        .Select(p => new ProductServiceModel
                //        {
                //            Id = p.Id,
                //            Name = p.Name,
                //            Category = new CategoryServiceModel
                //            {
                //                Name = p.Category.Name
                //            },
                //            ProductType = new ProductTypeServiceModel
                //            {
                //                Name = p.ProductType.Name
                //            },
                //            Description = p.Description,
                //            Pictures = p.Pictures
                //                .Select(pic => new PictureServiceModel
                //                {
                //                    Id = pic.Id,
                //                    PictureUrl = pic.PictureUrl
                //                })
                //                .ToList(),
                //            Colour = p.Colour,
                //            Size = p.Size,
                //            Manufacturer = new ManufacturerServiceModel
                //            {
                //                Id = p.ManufacturerId,
                //                Name = p.Manufacturer.Name
                //            },
                //            Price = p.Price,
                //            Quantity = new QuantityServiceModel
                //            {
                //                Id = p.QuantityId,
                //                AvailableItems = p.Quantity.AvailableItems
                //            },
                //            SaleId = p.SaleId,
                //            ShoppingCartId = p.ShoppingCartId,
                //            OrderId = p.OrderId,
                //            Reviews = p.Reviews
                //                .Select(r => new CustomerReviewServiceModel
                //                {
                //                    Id = r.Id,
                //                    Title = r.Title,
                //                    Text = r.Text,
                //                    CreatedOn = r.CreatedOn.ToString("dddd, dd MMMM yyyy"),
                //                    AuthorUsername = r.Author.FullName
                //                })
                //                .ToList()
                //        });
                //}
            //else
            //{
            //    string criteriaToLower = criteria.ToLower();

            //    allSearchResults = this.db.Products
            //        .Include(p => p.Category)
            //        .Include(p => p.ProductType)
            //        .Include(p => p.Pictures)
            //        .Include(p => p.Manufacturer)
            //        .Include(p => p.Reviews)
            //        .Where(p => p.Name.ToLower().Contains(criteriaToLower)
            //        || p.Description.ToLower().Contains(criteriaToLower)
            //        || p.Manufacturer.Name.ToLower().Contains(criteriaToLower)
            //        || p.Colour.ToLower().Contains(criteriaToLower)
            //        || p.Size.ToLower().Contains(criteriaToLower)
            //        || p.Sale.Title.ToLower().Contains(criteriaToLower)
            //        || p.Category.Name.ToLower().Contains(criteriaToLower)
            //        || p.ProductType.Name.ToLower().Contains(criteriaToLower))
            //        .Select(p => new ProductServiceModel
            //        {
            //            Id = p.Id,
            //            Name = p.Name,
            //            Category = new CategoryServiceModel
            //            {
            //                Name = p.Category.Name
            //            },
            //            ProductType = new ProductTypeServiceModel
            //            {
            //                Name = p.ProductType.Name
            //            },
            //            Description = p.Description,
            //            Pictures = p.Pictures
            //                .Select(pic => new PictureServiceModel
            //                {
            //                    Id = pic.Id,
            //                    PictureUrl = pic.PictureUrl
            //                })
            //                .ToList(),
            //            Colour = p.Colour,
            //            Size = p.Size,
            //            Manufacturer = new ManufacturerServiceModel
            //            {
            //                Id = p.ManufacturerId,
            //                Name = p.Manufacturer.Name
            //            },
            //            Price = p.Price,
            //            Quantity = new QuantityServiceModel
            //            {
            //                Id = p.QuantityId,
            //                AvailableItems = p.Quantity.AvailableItems
            //            },
            //            SaleId = p.SaleId,
            //            ShoppingCartId = p.ShoppingCartId,
            //            OrderId = p.OrderId,
            //            Reviews = p.Reviews
            //                .Select(r => new CustomerReviewServiceModel
            //                {
            //                    Id = r.Id,
            //                    Title = r.Title,
            //                    Text = r.Text,
            //                    CreatedOn = r.CreatedOn.ToString("dddd, dd MMMM yyyy"),
            //                    AuthorUsername = r.Author.FullName
            //                })
            //                .ToList()
            //        });
            //}

            return allSearchResults;
        }
    }
}
