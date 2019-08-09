﻿using DaysForGirls.Data;
using DaysForGirls.Data.Models;
using DaysForGirls.Services.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaysForGirls.Services
{
    public class AdminService : IAdminService
    {
        private readonly DaysForGirlsDbContext db;
        private readonly IPictureService pictureService;

        public AdminService(
            DaysForGirlsDbContext db,
            IPictureService pictureService)
        {
            this.db = db;
            this.pictureService = pictureService;
        }

        public async Task<int> CreateAsync(ProductServiceModel productServiceModel)
        {
            ProductType productTypeInDb = db.ProductTypes
                .SingleOrDefault(pT => pT.Name == productServiceModel.ProductType.Name);

            Category categoryInDb = this.db.Categories
                .SingleOrDefault(cat => cat.Name == productServiceModel.Category.Name);

            Manufacturer manufacturerInDb = this.db.Manufacturers
                .SingleOrDefault(man => man.Name == productServiceModel.Manufacturer.Name);

            Product product = new Product
            {
                Name = productServiceModel.Name,
                ProductType = productTypeInDb,
                Category = categoryInDb,
                Description = productServiceModel.Description,
                Colour = productServiceModel.Colour,
                Size = productServiceModel.Size,
                Price = productServiceModel.Price,
                Manufacturer = manufacturerInDb,
                QuantityId = productServiceModel.Quantity.AvailableItems,
                Carts = new List<ProductCart>(),
                Reviews = new List<CustomerReview>(),
                Pictures = new List<Picture>(),
                Sales = new List<ProductSale>()
            };

            product.Pictures = productServiceModel.Pictures
                .Select(p => new Picture
                {
                    PictureUrl = p.PictureUrl
                })
                .ToList();

            this.db.Products.Add(product);
            await db.SaveChangesAsync();

            int productId = product.Id;

            return productId;
        }

        public IQueryable<AdminProductAllServiceModel> DisplayAll()
        {
            var allProducts = this.db.Products
                .Select(p => new AdminProductAllServiceModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Category = new CategoryServiceModel
                    {
                        Name = p.Category.Name
                    },
                    Picture = new PictureServiceModel
                    {
                        PictureUrl = p.Pictures.ElementAt(0).PictureUrl
                    },
                    Price = p.Price,
                    Manufacturer = new ManufacturerServiceModel
                    {
                        Name = p.Manufacturer.Name
                    },
                    IsDeleted = p.IsDeleted,
                    IsInSale = p.IsInSale
                });

            return allProducts;
        }

        public async Task<ProductServiceModel> GetProductByIdAsync(int productId)
        {
            var product = await this.db.Products
                .Include(p => p.Category)
                .Include(p => p.ProductType)
                .Include(p => p.Manufacturer)
                .Include(p => p.Quantity)
                .SingleOrDefaultAsync(p => p.Id == productId);

            var productPictures = await this.db.Pictures
                .Where(pic => pic.ProductId == product.Id)
                    .Select(pic => new PictureServiceModel
                    {
                        Id = pic.Id,
                        PictureUrl = pic.PictureUrl
                    }).ToListAsync();

            var productReviews = await this.db.CustomerReviews
                .Where(cR => cR.ProductId == product.Id)
                .Select(pR => new CustomerReviewServiceModel
                {
                    Id = pR.Id,
                    Title = pR.Title,
                    Text = pR.Text,
                    CreatedOn = pR.CreatedOn.ToString("dddd, dd MMMM yyyy"),
                    AuthorUsername = pR.Author.UserName
                }).ToListAsync();

            var productSales = await this.db.ProductsSales
                .Where(s => s.ProductId == product.Id)
                .Select(s => new ProductSaleServiceModel
                {
                    Id = s.Id,
                    SaleId = s.SaleId
                }).ToListAsync();

            var productCategory = await this.db.Categories
                .SingleOrDefaultAsync(c => c.Id == product.CategoryId);

            ProductServiceModel productToReturn = new ProductServiceModel
            {
                Id = product.Id,
                Name = product.Name,
                ProductType = new ProductTypeServiceModel
                {
                    Name = product.ProductType.Name
                },
                Category = new CategoryServiceModel
                {
                    Name = product.Category.Name
                },
                Description = product.Description,
                Pictures = productPictures,
                Colour = product.Colour,
                Size = product.Size,
                Price = product.Price,
                Manufacturer = new ManufacturerServiceModel
                {
                    Name = product.Manufacturer.Name
                },
                Quantity = new QuantityServiceModel
                {
                    AvailableItems = product.Quantity.AvailableItems
                },
                Reviews = productReviews,
                IsDeleted = product.IsDeleted,
                Sales = productSales
            };

            await Task.Delay(0);
            return productToReturn;
        }

        public async Task<bool> DeleteProductByIdAsync(int id)
        {
            var productToDelete = this.db.Products
                .SingleOrDefault(product => product.Id == id);

            productToDelete.IsDeleted = true;

            bool picturesAreDeleted = await this.pictureService
                .DeletePicturesOfDeletedProduct(productToDelete.Id);

            this.db.Update(productToDelete);
            int result = await this.db.SaveChangesAsync();
            bool productIsDeleted = result > 0;

            return productIsDeleted;
        }

        public async Task<bool> EditAsync(int productId, ProductServiceModel model)
        {
            ProductType productTypeOfProduct = this.db.ProductTypes
                .SingleOrDefault(pT => pT.Name == model.ProductType.Name);

            Category categoryOfProduct = this.db.Categories
                .SingleOrDefault(c => c.Name == model.Category.Name);

            Manufacturer manufacturerOfProduct = this.db.Manufacturers
                .SingleOrDefault(m => m.Name == model.Manufacturer.Name);

            if (productTypeOfProduct == null
                || categoryOfProduct == null
                || manufacturerOfProduct == null)
            {
                throw new ArgumentNullException(nameof(productTypeOfProduct));
            }

            Product productInDb = this.db.Products
                .SingleOrDefault(p => p.Id == productId);

            productInDb.Name = model.Name;
            productInDb.ProductType = productTypeOfProduct;
            productInDb.Category = categoryOfProduct;
            productInDb.Description = model.Description;
            productInDb.Colour = model.Colour;
            productInDb.Size = model.Size;
            productInDb.Price = model.Price;
            productInDb.Manufacturer = manufacturerOfProduct;
            productInDb.QuantityId = model.Quantity.Id;

            this.db.Products.Update(productInDb);
            int result = await db.SaveChangesAsync();

            bool editsApplied = result > 0;

            return editsApplied;
        }

        public async Task<bool> UploadNewPictureToProduct(int productId, string imageUrl)
        {
            var productInDb = this.db.Products.
                SingleOrDefault(p => p.Id == productId);

            Picture newPicture = new Picture
            {
                PictureUrl = imageUrl
            };

            productInDb.Pictures.Add(newPicture);
            this.db.Update(productInDb);
            int result = await this.db.SaveChangesAsync();

            bool pictureIsAdded = result > 0;

            return pictureIsAdded;
        }

        public async Task<bool> AddProductToSaleAsync(int productId, int saleId)
        {
            var productSale = this.db.ProductsSales
                .SingleOrDefault(pS => pS.ProductId == productId
                && pS.SaleId == saleId);

            var product = this.db.Products
                .SingleOrDefault(p => p.Id == productId);

            product.Sales.Add(productSale);
            product.IsInSale = true;

            this.db.Products.Update(product);
            int result = await this.db.SaveChangesAsync();
            bool productIsAddedToSale = result > 0;

            return productIsAddedToSale;
        }

        public async Task<bool> UploadNewPictureToProductAsync(int productId, string imageUrl)
        {
            var productInDb = this.db.Products
                .SingleOrDefault(p => p.Id == productId);

            Picture newPicture = new Picture
            {
                PictureUrl = imageUrl
            };

            productInDb.Pictures.Add(newPicture);
            this.db.Update(productInDb);
            int result = await this.db.SaveChangesAsync();

            bool pictureIsAdded = result > 0;

            return pictureIsAdded;
        }
    }
}
