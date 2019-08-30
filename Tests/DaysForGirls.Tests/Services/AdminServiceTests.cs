using DaysForGirls.Data;
using DaysForGirls.Data.Models;
using DaysForGirls.Services;
using DaysForGirls.Services.Models;
using DaysForGirls.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DaysForGirls.Tests.Services
{
    public class AdminServiceTests
    {
        private IAdminService adminService;
        private IPictureService pictureService;
        private ICustomerReviewService customerReviewService;

        private List<Product> GetSampleProducts()
        {
            return new List<Product>()
            {
                new Product
                {
                    Name = "Product One",
                    Description = "Product One Description",
                    Category = new Category
                    {
                        Name = "Wedding",
                        Description = "About the wedding"
                    },
                    ProductType = new ProductType
                    {
                        Name = "Dress"
                    },
                    Colour = "White",
                    Size = "M",
                    Manufacturer = new Manufacturer
                    {
                        Name = "Versace",
                        Description = "Italiano",
                        Logo = new Logo{ LogoUrl = "Versace_Logo"}
                    },
                    Pictures = new List<Picture>()
                    {
                        new Picture
                        {
                            PictureUrl = "Versace_1"
                        },
                        new Picture
                        {
                            PictureUrl = "Versace_2"
                        }
                    },
                    Price = 100.50M,
                    Quantity = new Quantity{ AvailableItems = 1 }
                },
                new Product
                {
                    Name = "Product Two",
                    Description = "Product Two Description",
                    Category = new Category
                    {
                        Name = "Prom",
                        Description = "About the prom"
                    },
                    ProductType = new ProductType
                    {
                        Name = "Accessory"
                    },
                    Colour = "Blue",
                    Size = "20 cm",
                    Manufacturer = new Manufacturer
                    {
                        Name = "Diesel",
                        Description = "For the young",
                        Logo = new Logo{ LogoUrl = "Diesel_Logo"}
                    },
                    Pictures = new List<Picture>()
                    {
                        new Picture
                        {
                            PictureUrl = "Diesel_1"
                        },
                        new Picture
                        {
                            PictureUrl = "Diesel_2"
                        }
                    },
                    Price = 20.50M,
                    Quantity = new Quantity{ AvailableItems = 1 }
                }
            };
        }

        private async Task SeedSampleProducts(DaysForGirlsDbContext db)
        {
            db.AddRange(GetSampleProducts());
            await db.SaveChangesAsync();
        }

        [Fact]
        public async Task Create_WithCorrectDataAndNotInSale_ExpectedToCreateAProduct()
        {
            string errorMessagePrefix = "ProductService CreateAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            var pictureService = new PictureService(db);
            var customerReviewService = new CustomerReviewService(db);
            this.adminService = new AdminService(db, pictureService, customerReviewService);

            ProductType type = new ProductType { Name = "Dress" };
            db.ProductTypes.Add(type);
            Category category = new Category
            {
                Name = "Wedding",
                Description = "Wedding description"
            };
            db.Categories.Add(category);
            Manufacturer manufacturer = new Manufacturer
            {
                Name = "Manufacturer",
                Description = "Manufacturer Description",
                Logo = new Logo { LogoUrl = "Manuf_Logo" }
            };
            db.Manufacturers.Add(manufacturer);
            Quantity quantity = new Quantity { AvailableItems = 1 };
            db.Quantities.Add(quantity);
            await db.SaveChangesAsync();

            ProductServiceModel testProduct = new ProductServiceModel
            {
                Name = "Product Three",
                Description = "Product Three description",
                Colour = "Green",
                Size = "Medium",
                Category = new CategoryServiceModel
                {
                    Name = category.Name
                },
                ProductType = new ProductTypeServiceModel
                {
                    Name = type.Name
                },
                Manufacturer = new ManufacturerServiceModel
                {
                    Name = manufacturer.Name
                },
                Pictures = new List<PictureServiceModel>()
                {
                    new PictureServiceModel
                    {
                        PictureUrl = "Prod_Three_1"
                    }
                },
                Price = 220.00M,
                Quantity = new QuantityServiceModel
                {
                    AvailableItems = quantity.AvailableItems
                }
            };

            int actualResult = await this.adminService.CreateAsync(testProduct);
            Assert.True(actualResult > 0, errorMessagePrefix);
        }

        [Fact]
        public async Task Create_WithNonExistentProductType_ExpectedToCreateAProduct()
        {
            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            var pictureService = new PictureService(db);
            var customerReviewService = new CustomerReviewService(db);
            this.adminService = new AdminService(db, pictureService, customerReviewService);

            Category category = new Category
            {
                Name = "Wedding",
                Description = "Wedding description"
            };
            db.Categories.Add(category);
            Manufacturer manufacturer = new Manufacturer
            {
                Name = "Manufacturer",
                Description = "Manufacturer Description",
                Logo = new Logo { LogoUrl = "Manuf_Logo" }
            };
            db.Manufacturers.Add(manufacturer);
            Quantity quantity = new Quantity { AvailableItems = 1 };
            db.Quantities.Add(quantity);
            await db.SaveChangesAsync();

            ProductServiceModel testProduct = new ProductServiceModel
            {
                Name = "Product Three",
                Description = "Product Three description",
                Colour = "Green",
                Size = "Medium",
                Category = new CategoryServiceModel
                {
                    Name = category.Name
                },
                ProductType = new ProductTypeServiceModel
                {
                    Name = "Dress"
                },
                Manufacturer = new ManufacturerServiceModel
                {
                    Name = manufacturer.Name
                },
                Pictures = new List<PictureServiceModel>()
                {
                    new PictureServiceModel
                    {
                        PictureUrl = "Prod_Three_1"
                    }
                },
                Price = 220.00M,
                Quantity = new QuantityServiceModel
                {
                    AvailableItems = quantity.AvailableItems
                }
            };

            await Assert.ThrowsAsync<ArgumentNullException>(() => this.adminService.CreateAsync(testProduct));
        }

        [Fact]
        public async Task DisplayAll_WithSampleData_ExpectedToReturnAllRegisteredProducts()
        {
            string errorMessagePrefix = "ManufacturerService DisplayAll() method does not work properly.";
            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();

            await SeedSampleProducts(db);

            var pictureService = new PictureService(db);
            var customerReviewService = new CustomerReviewService(db);
            this.adminService = new AdminService(db, pictureService, customerReviewService);

            List<AdminProductAllServiceModel> expectedResults = GetSampleProducts()
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
                    AvailableItems = p.Quantity.AvailableItems,
                    Manufacturer = new ManufacturerServiceModel
                    {
                        Name = p.Manufacturer.Name
                    },
                    IsDeleted = p.IsDeleted,
                    IsInSale = p.IsInSale,
                    SaleId = p.SaleId,
                    ShoppingCartId = p.ShoppingCartId,
                    OrderId = p.OrderId
                }).ToList();

            List<AdminProductAllServiceModel> actualResults = await this.adminService.DisplayAll().ToListAsync();


            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedRecord = expectedResults[i];
                var actualRecord = actualResults[i];

                Assert.True(expectedRecord.Name == actualRecord.Name, errorMessagePrefix + " " + "Name is not returned properly.");
                Assert.True(expectedRecord.Category.Name == actualRecord.Category.Name, errorMessagePrefix + " " + "Category Name is not returned properly.");
                Assert.True(expectedRecord.Picture.PictureUrl == actualRecord.Picture.PictureUrl, errorMessagePrefix + " " + "Picture is not returned properly.");
                Assert.True(expectedRecord.Price == actualRecord.Price, errorMessagePrefix + " " + "Price is not returned properly.");
                Assert.True(expectedRecord.AvailableItems == actualRecord.AvailableItems, errorMessagePrefix + " " + "Available Items is not returned properly.");
                Assert.True(expectedRecord.Manufacturer.Name == actualRecord.Manufacturer.Name, errorMessagePrefix + " " + "Manufacturer Name is not returned properly.");
                Assert.True(expectedRecord.IsDeleted == actualRecord.IsDeleted, errorMessagePrefix + " " + "IsDeleted is not returned properly.");
                Assert.True(expectedRecord.IsInSale == actualRecord.IsInSale, errorMessagePrefix + " " + "IsInSale is not returned properly.");
                Assert.True(expectedRecord.SaleId == actualRecord.SaleId, errorMessagePrefix + " " + "Sale Id is not returned properly.");
                Assert.True(expectedRecord.ShoppingCartId == actualRecord.ShoppingCartId, errorMessagePrefix + " " + "ShoppingCart Id is not returned properly.");
                Assert.True(expectedRecord.OrderId == actualRecord.OrderId, errorMessagePrefix + " " + "Order Id is not returned properly.");
            }
        }

        [Fact]
        public async Task DisplayAll_WithZeroData_ExpectedAnEmptyList()
        {
            string errorMessagePrefix = "ManufacturerService DisplayAll() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            var pictureService = new PictureService(db);
            var customerReviewService = new CustomerReviewService(db);
            this.adminService = new AdminService(db, pictureService, customerReviewService);

            List<AdminProductAllServiceModel> actualResults = await this.adminService.DisplayAll().ToListAsync();

            Assert.True(actualResults.Count == 0, errorMessagePrefix);
        }

        [Fact]
        public async Task GetById_WithExistingId_ShouldReturnAProduct()
        {
            string errorMessagePrefix = "ManufacturerService GetManufacturerByIdAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            var pictureService = new PictureService(db);
            var customerReviewService = new CustomerReviewService(db);
            this.adminService = new AdminService(db, pictureService, customerReviewService);
            await SeedSampleProducts(db);

            this.adminService = new AdminService(db, pictureService, customerReviewService);

            Product expectedData = db.Products.First();

            ProductServiceModel expectedDataServiceModel = new ProductServiceModel
            {
                Id = expectedData.Id,
                Name = expectedData.Name,
                Description = expectedData.Description,
                Category = new CategoryServiceModel { Name = expectedData.Category.Name },
                ProductType = new ProductTypeServiceModel { Name = expectedData.ProductType.Name },
                Colour = expectedData.Colour,
                Size = expectedData.Size,
                Manufacturer = new ManufacturerServiceModel { Name = expectedData.Manufacturer.Name },
                Price = expectedData.Price,
                OrderId = expectedData.OrderId,
                SaleId = expectedData.SaleId,
                ShoppingCartId = expectedData.ShoppingCartId,
                Quantity = new QuantityServiceModel { AvailableItems = expectedData.Quantity.AvailableItems },
                Pictures = expectedData.Pictures
                    .Select(pic => new PictureServiceModel
                    {
                        PictureUrl = pic.PictureUrl
                    }).ToList(),
                Reviews = expectedData.Reviews
                    .Select(r => new CustomerReviewServiceModel
                    {
                        Id = r.Id,
                        Title = r.Title,
                        Text = r.Text,
                        AuthorId = r.AuthorId,
                        CreatedOn = r.CreatedOn.ToString("dddd, dd MMMM yyyy"),
                        IsDeleted = r.IsDeleted
                    }).ToList(),
                IsDeleted = expectedData.IsDeleted
            };

            ProductServiceModel actualRecord = await this.adminService.GetProductByIdAsync(expectedData.Id);

            Assert.True(expectedDataServiceModel.Name == actualRecord.Name, errorMessagePrefix + " " + "Name is not returned properly.");
            Assert.True(expectedDataServiceModel.Category.Name == actualRecord.Category.Name, errorMessagePrefix + " " + "Category Name is not returned properly.");
            Assert.True(expectedDataServiceModel.Pictures.Count() == actualRecord.Pictures.Count(), errorMessagePrefix + " " + "Picture is not returned properly.");
            Assert.True(expectedDataServiceModel.Price == actualRecord.Price, errorMessagePrefix + " " + "Price is not returned properly.");
            Assert.True(expectedDataServiceModel.Quantity.AvailableItems == actualRecord.Quantity.AvailableItems, errorMessagePrefix + " " + "Available Items is not returned properly.");
            Assert.True(expectedDataServiceModel.Manufacturer.Name == actualRecord.Manufacturer.Name, errorMessagePrefix + " " + "Manufacturer Name is not returned properly.");
            Assert.True(expectedDataServiceModel.IsDeleted == actualRecord.IsDeleted, errorMessagePrefix + " " + "IsDeleted is not returned properly.");
            Assert.True(expectedDataServiceModel.IsInSale == actualRecord.IsInSale, errorMessagePrefix + " " + "IsInSale is not returned properly.");
            Assert.True(expectedDataServiceModel.SaleId == actualRecord.SaleId, errorMessagePrefix + " " + "Sale Id is not returned properly.");
            Assert.True(expectedDataServiceModel.ShoppingCartId == actualRecord.ShoppingCartId, errorMessagePrefix + " " + "ShoppingCart Id is not returned properly.");
            Assert.True(expectedDataServiceModel.OrderId == actualRecord.OrderId, errorMessagePrefix + " " + "Order Id is not returned properly.");
        }

        [Fact]
        public async Task GetById_WithNonexistentId_ShouldThrowInvalidOperationException()
        {
            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            var pictureService = new PictureService(db);
            var customerReviewService = new CustomerReviewService(db);
            this.adminService = new AdminService(db, pictureService, customerReviewService);
            await SeedSampleProducts(db);

            await Assert.ThrowsAsync<ArgumentNullException>(() => this.adminService.GetProductByIdAsync(8));
        }

        [Fact]
        public async Task Edit_WithCorrectData_ExpectedCorrectlyEditedProduct()
        {
            string errorMessagePrefix = "AdminService EditAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            var pictureService = new PictureService(db);
            var customerReviewService = new CustomerReviewService(db);
            this.adminService = new AdminService(db, pictureService, customerReviewService);
            await SeedSampleProducts(db);

            this.adminService = new AdminService(db, pictureService, customerReviewService);

            Product expectedData = db.Products.First();

            ProductServiceModel expectedDataServiceModel = new ProductServiceModel
            {
                Id = expectedData.Id,
                Name = expectedData.Name,
                Description = expectedData.Description,
                Category = new CategoryServiceModel { Name = expectedData.Category.Name },
                ProductType = new ProductTypeServiceModel { Name = expectedData.ProductType.Name },
                Colour = expectedData.Colour,
                Size = expectedData.Size,
                Manufacturer = new ManufacturerServiceModel { Name = expectedData.Manufacturer.Name },
                Price = expectedData.Price,
                OrderId = expectedData.OrderId,
                SaleId = expectedData.SaleId,
                ShoppingCartId = expectedData.ShoppingCartId,
                Quantity = new QuantityServiceModel { AvailableItems = expectedData.Quantity.AvailableItems },
                Pictures = expectedData.Pictures
                    .Select(pic => new PictureServiceModel
                    {
                        PictureUrl = pic.PictureUrl
                    }).ToList(),
                Reviews = expectedData.Reviews
                    .Select(r => new CustomerReviewServiceModel
                    {
                        Id = r.Id,
                        Title = r.Title,
                        Text = r.Text,
                        AuthorId = r.AuthorId,
                        CreatedOn = r.CreatedOn.ToString("dddd, dd MMMM yyyy"),
                        IsDeleted = r.IsDeleted
                    }).ToList(),
                IsDeleted = expectedData.IsDeleted
            };

            expectedDataServiceModel.Name = "New Product Name";

            await this.adminService.EditAsync(expectedDataServiceModel);

            var actualRecord = db.Products.First();

            var actualRecordServiceModel = new ProductServiceModel
            {
                Id = actualRecord.Id,
                Name = actualRecord.Name,
                Description = actualRecord.Description,
                Category = new CategoryServiceModel { Name = actualRecord.Category.Name },
                ProductType = new ProductTypeServiceModel { Name = actualRecord.ProductType.Name },
                Colour = actualRecord.Colour,
                Size = actualRecord.Size,
                Manufacturer = new ManufacturerServiceModel { Name = actualRecord.Manufacturer.Name },
                Price = actualRecord.Price,
                OrderId = actualRecord.OrderId,
                SaleId = actualRecord.SaleId,
                ShoppingCartId = actualRecord.ShoppingCartId,
                Quantity = new QuantityServiceModel { AvailableItems = actualRecord.Quantity.AvailableItems },
                Pictures = actualRecord.Pictures
                    .Select(pic => new PictureServiceModel
                    {
                        PictureUrl = pic.PictureUrl
                    }).ToList(),
                Reviews = actualRecord.Reviews
                    .Select(r => new CustomerReviewServiceModel
                    {
                        Id = r.Id,
                        Title = r.Title,
                        Text = r.Text,
                        AuthorId = r.AuthorId,
                        CreatedOn = r.CreatedOn.ToString("dddd, dd MMMM yyyy"),
                        IsDeleted = r.IsDeleted
                    }).ToList(),
                IsDeleted = actualRecord.IsDeleted
            };

            Assert.True(expectedDataServiceModel.Name == actualRecordServiceModel.Name, errorMessagePrefix + " " + "Name is not returned properly.");
            Assert.True(expectedDataServiceModel.Category.Name == actualRecordServiceModel.Category.Name, errorMessagePrefix + " " + "Category Name is not returned properly.");
            Assert.True(expectedDataServiceModel.Pictures.Count() == actualRecordServiceModel.Pictures.Count(), errorMessagePrefix + " " + "Picture is not returned properly.");
            Assert.True(expectedDataServiceModel.Price == actualRecordServiceModel.Price, errorMessagePrefix + " " + "Price is not returned properly.");
            Assert.True(expectedDataServiceModel.Quantity.AvailableItems == actualRecordServiceModel.Quantity.AvailableItems, errorMessagePrefix + " " + "Available Items is not returned properly.");
            Assert.True(expectedDataServiceModel.Manufacturer.Name == actualRecordServiceModel.Manufacturer.Name, errorMessagePrefix + " " + "Manufacturer Name is not returned properly.");
            Assert.True(expectedDataServiceModel.IsDeleted == actualRecordServiceModel.IsDeleted, errorMessagePrefix + " " + "IsDeleted is not returned properly.");
            Assert.True(expectedDataServiceModel.SaleId == actualRecordServiceModel.SaleId, errorMessagePrefix + " " + "Sale Id is not returned properly.");
            Assert.True(expectedDataServiceModel.ShoppingCartId == actualRecordServiceModel.ShoppingCartId, errorMessagePrefix + " " + "ShoppingCart Id is not returned properly.");
            Assert.True(expectedDataServiceModel.OrderId == actualRecordServiceModel.OrderId, errorMessagePrefix + " " + "Order Id is not returned properly.");
        }

        [Fact]
        public async Task DeleteProductById_WithExistingIdNotSaleCartOrOrder_ExpectedResultShouldContainTrue()
        {
            string errorMessagePrefix = "AdminService EraseFromDb() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            var pictureService = new PictureService(db);
            var customerReviewService = new CustomerReviewService(db);
            this.adminService = new AdminService(db, pictureService, customerReviewService);
            await SeedSampleProducts(db);

            this.adminService = new AdminService(db, pictureService, customerReviewService);

            Product product = db.Products.First();

            string actualResult = await this.adminService.EraseFromDb(product.Id);
            string[] actualResultSplit = actualResult.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.True(actualResultSplit[0] == product.Name, errorMessagePrefix + " " + "Resukt does not contain the product name.");
            Assert.True(actualResultSplit[1] == "true", errorMessagePrefix + " " + "Result does not contain the word True.");
        }

        [Fact]
        public async Task DeleteProductById_WithExistingIdInSaleNoCartOrOrder_ExpectedResultShouldContainTrue()
        {
            string errorMessagePrefix = "AdminService EraseFromDb() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            var pictureService = new PictureService(db);
            var customerReviewService = new CustomerReviewService(db);
            this.adminService = new AdminService(db, pictureService, customerReviewService);
            Sale sale = new Sale
            {
                Title = "Sale",
                EndsOn = DateTime.UtcNow.AddDays(20),
                Picture = "Sale_Picture"
            };

            db.Sales.Add(sale);
            await db.SaveChangesAsync();

            Product product = new Product
            {
                Name = "Product One",
                Category = new Category
                {
                    Name = "Haha",
                    Description = "Haha is great"
                },
                ProductType = new ProductType { Name = "Gashti" },
                Manufacturer = new Manufacturer
                {
                    Name = "Armani",
                    Description = "About Armani",
                    Logo = new Logo { LogoUrl = "Armani_Logo" }
                },
                Colour = "Green",
                Size = "Fit",
                OrderId = null,
                SaleId = sale.Id,
                ShoppingCart = null,
                Price = 20.00m,
                Quantity = new Quantity { AvailableItems = 1 },
                Pictures = new List<Picture>()
                {
                    new Picture{ PictureUrl = "a" },
                    new Picture{ PictureUrl = "b" }
                }
            };

            db.Products.Add(product);
            await db.SaveChangesAsync();

            this.adminService = new AdminService(db, pictureService, customerReviewService);

            Product productToDelete = db.Products.First();

            string actualResult = await this.adminService.EraseFromDb(product.Id);
            string[] actualResultSplit = actualResult.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.True(actualResultSplit[0] == product.Name, errorMessagePrefix + " " + "Resukt does not contain the product name.");
            Assert.True(actualResultSplit[1] == "true", errorMessagePrefix + " " + "Result does not contain the word True.");
        }

        [Fact]
        public async Task DeleteProductById_WithExistingIdInSaleInCartNoOrder_ExpectedResultShouldNotDeleteProduct()
        {
            string errorMessagePrefix = "AdminService EraseFromDb() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            var pictureService = new PictureService(db);
            var customerReviewService = new CustomerReviewService(db);
            this.adminService = new AdminService(db, pictureService, customerReviewService);
            Sale sale = new Sale
            {
                Title = "Sale",
                EndsOn = DateTime.UtcNow.AddDays(20),
                Picture = "Sale_Picture"
            };

            db.Sales.Add(sale);
            await db.SaveChangesAsync();

            Product product = new Product
            {
                Name = "Product One",
                Category = new Category
                {
                    Name = "Haha",
                    Description = "Haha is great"
                },
                ProductType = new ProductType { Name = "Gashti" },
                Manufacturer = new Manufacturer
                {
                    Name = "Armani",
                    Description = "About Armani",
                    Logo = new Logo { LogoUrl = "Armani_Logo" }
                },
                Colour = "Green",
                Size = "Fit",
                OrderId = null,
                SaleId = sale.Id,
                ShoppingCartId = "Hi",
                Price = 20.00m,
                Quantity = new Quantity { AvailableItems = 1 },
                Pictures = new List<Picture>()
                {
                    new Picture{ PictureUrl = "a" },
                    new Picture{ PictureUrl = "b" }
                }
            };

            db.Products.Add(product);
            await db.SaveChangesAsync();

            this.adminService = new AdminService(db, pictureService, customerReviewService);

            Product productToDelete = db.Products.First();

            string actualResult = await this.adminService.EraseFromDb(product.Id);
            string[] actualResultSplit = actualResult.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.True(actualResultSplit[0] == product.Name, errorMessagePrefix + " " + "Resukt does not contain the product name.");
            Assert.True(actualResultSplit[1] == "is in a Shopping Cart and cannot be deleted.", errorMessagePrefix + " " + "Result does not contain the expected message.");
        }

        [Fact]
        public async Task DeleteProductById_WithExistingIdInSaleInOrder_ExpectedResultShouldSetIsDeletedToTrue()
        {
            string errorMessagePrefix = "AdminService EraseFromDb() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            var pictureService = new PictureService(db);
            var customerReviewService = new CustomerReviewService(db);
            this.adminService = new AdminService(db, pictureService, customerReviewService);
            Sale sale = new Sale
            {
                Title = "Sale",
                EndsOn = DateTime.UtcNow.AddDays(20),
                Picture = "Sale_Picture"
            };

            db.Sales.Add(sale);
            await db.SaveChangesAsync();

            Product product = new Product
            {
                Name = "Product One",
                Category = new Category
                {
                    Name = "Haha",
                    Description = "Haha is great"
                },
                ProductType = new ProductType { Name = "Gashti" },
                Manufacturer = new Manufacturer
                {
                    Name = "Armani",
                    Description = "About Armani",
                    Logo = new Logo { LogoUrl = "Armani_Logo" }
                },
                Colour = "Green",
                Size = "Fit",
                OrderId = "Hi",
                SaleId = sale.Id,
                ShoppingCartId = null,
                Price = 20.00m,
                Quantity = new Quantity { AvailableItems = 1 },
                Pictures = new List<Picture>()
                {
                    new Picture{ PictureUrl = "a" },
                    new Picture{ PictureUrl = "b" }
                }
            };

            db.Products.Add(product);
            await db.SaveChangesAsync();

            this.adminService = new AdminService(db, pictureService, customerReviewService);

            Product productToDelete = db.Products.First();

            string actualResult = await this.adminService.EraseFromDb(product.Id);
            string[] actualResultSplit = actualResult.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.True(actualResultSplit[0] == product.Name, errorMessagePrefix + " " + "Result does not contain the product name.");
            Assert.True(actualResultSplit[1] == "has been purchased and was only set to IsDeleted.", errorMessagePrefix + " " + "Result does not return the expected message.");
            Assert.True(productToDelete.IsDeleted, errorMessagePrefix + " " + "IsDeleted is not set to true");
        }

        [Fact]
        public async Task SetOrderId_WithExistingProductId_ExpectedResultShouldSetCartIdToNullAndGivenOrderId()
        {
            string errorMessagePrefix = "AdminService SetOrderId() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            var pictureService = new PictureService(db);
            var customerReviewService = new CustomerReviewService(db);
            this.adminService = new AdminService(db, pictureService, customerReviewService);

            Product product = new Product
            {
                Name = "Product One",
                Category = new Category
                {
                    Name = "Haha",
                    Description = "Haha is great"
                },
                ProductType = new ProductType { Name = "Gashti" },
                Manufacturer = new Manufacturer
                {
                    Name = "Armani",
                    Description = "About Armani",
                    Logo = new Logo { LogoUrl = "Armani_Logo" }
                },
                Colour = "Green",
                Size = "Fit",
                OrderId = null,
                SaleId = null,
                ShoppingCartId = "yes",
                Price = 20.00m,
                Quantity = new Quantity { AvailableItems = 1 },
                Pictures = new List<Picture>()
                {
                    new Picture{ PictureUrl = "a" },
                    new Picture{ PictureUrl = "b" }
                }
            };

            db.Products.Add(product);
            await db.SaveChangesAsync();

            this.adminService = new AdminService(db, pictureService, customerReviewService);

            List<int> productIds = db.Products
                .Select(p => p.Id).ToList();

            bool positive = await this.adminService.SetOrderIdToProductsAsync(productIds, "Hi");
            Product first = db.Products.SingleOrDefault(p => p.Id == 1);

            Assert.True(positive, errorMessagePrefix + " " + "Products were updated with OrderId");
            Assert.True(first.OrderId == "Hi", errorMessagePrefix + " " + "OrderId not set to given Id.");
            Assert.True(first.ShoppingCartId == null, errorMessagePrefix + " " + "CartId not set to null");
        }

        [Fact]
        public async Task SetOrderId_WithoutProductIds_ExpectedResultShouldReturnFalse()
        {
            string errorMessagePrefix = "AdminService SetOrderId() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            var pictureService = new PictureService(db);
            var customerReviewService = new CustomerReviewService(db);
            this.adminService = new AdminService(db, pictureService, customerReviewService);

            Product product = new Product
            {
                Name = "Product One",
                Category = new Category
                {
                    Name = "Haha",
                    Description = "Haha is great"
                },
                ProductType = new ProductType { Name = "Gashti" },
                Manufacturer = new Manufacturer
                {
                    Name = "Armani",
                    Description = "About Armani",
                    Logo = new Logo { LogoUrl = "Armani_Logo" }
                },
                Colour = "Green",
                Size = "Fit",
                OrderId = null,
                SaleId = null,
                ShoppingCartId = "yes",
                Price = 20.00m,
                Quantity = new Quantity { AvailableItems = 1 },
                Pictures = new List<Picture>()
                {
                    new Picture{ PictureUrl = "a" },
                    new Picture{ PictureUrl = "b" }
                }
            };

            db.Products.Add(product);
            await db.SaveChangesAsync();

            this.adminService = new AdminService(db, pictureService, customerReviewService);

            List<int> productIds = new List<int>();

            bool positive = await this.adminService.SetOrderIdToProductsAsync(productIds, "Hi");
            Product first = db.Products.First();

            Assert.True(positive == false, errorMessagePrefix + " " + "OrderId not set and cartId remains not null.");
            Assert.True(first.OrderId == null, errorMessagePrefix + " " + "OrderId was set to given Id.");
            Assert.True(first.ShoppingCartId == "yes", errorMessagePrefix + " " + "CartId was set to null");
        }

        [Fact]
        public async Task SetOrderId_WithNullOrderId_ExpectedToThrowArgumentNullException()
        {
            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            var pictureService = new PictureService(db);
            var customerReviewService = new CustomerReviewService(db);
            this.adminService = new AdminService(db, pictureService, customerReviewService);

            Product product = new Product
            {
                Name = "Product One",
                Category = new Category
                {
                    Name = "Haha",
                    Description = "Haha is great"
                },
                ProductType = new ProductType { Name = "Gashti" },
                Manufacturer = new Manufacturer
                {
                    Name = "Armani",
                    Description = "About Armani",
                    Logo = new Logo { LogoUrl = "Armani_Logo" }
                },
                Colour = "Green",
                Size = "Fit",
                OrderId = null,
                SaleId = null,
                ShoppingCartId = "yes",
                Price = 20.00m,
                Quantity = new Quantity { AvailableItems = 1 },
                Pictures = new List<Picture>()
                {
                    new Picture{ PictureUrl = "a" },
                    new Picture{ PictureUrl = "b" }
                }
            };

            db.Products.Add(product);
            await db.SaveChangesAsync();

            this.adminService = new AdminService(db, pictureService, customerReviewService);

            List<int> productIds = db.Products
                .Select(p => p.Id).ToList();

            await Assert.ThrowsAsync<ArgumentNullException>(() => this.adminService.SetOrderIdToProductsAsync(productIds, null));
        }

        //[Fact]
        //public async Task DeleteProductById_WithNonexistentId_ExpectedToThrowArgumentNullException()
        //{
        //    var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
        //    var pictureService = new PictureService(db);
        //    var userManager = UserManagerMOQ.TestUserManager<DaysForGirlsUser>();
        //    var customerReviewService = new CustomerReviewService(userManager, db);
        //    this.adminService = new AdminService(db, pictureService, customerReviewService);
        //    await SeedSampleProducts(db);

        //    this.adminService = new AdminService(db, pictureService, customerReviewService);

        //    Product product = db.Products.First();

        //    await Assert.ThrowsAsync<ArgumentNullException>(() => this.adminService.GetProductByIdAsync(8));
        //}

        //[Fact]
        //public async Task UploadNewPictureToProduct_WithValidData_ExpectedToAddNewPictureToProduct()
        //{
        //    string errorMessagePrefix = "AdminService UploadNewPictureToProductAsync() method does not work properly.";

        //    var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
        //    var pictureService = new PictureService(db);
        //    var userManager = UserManagerMOQ.TestUserManager<DaysForGirlsUser>();
        //    var customerReviewService = new CustomerReviewService(userManager, db);
        //    this.adminService = new AdminService(db, pictureService, customerReviewService);
        //    await SeedSampleProducts(db);

        //    this.adminService = new AdminService(db, pictureService, customerReviewService);

        //    Product product = db.Products.First();

        //    string pictureUrl = "New Product Picture";

        //    bool pictureIsAdded = await this.adminService.UploadNewPictureToProductAsync(product.Id, pictureUrl);

        //    Assert.True(pictureIsAdded, errorMessagePrefix);
        //}

        //[Fact]
        //public async Task UploadNewPictureToProduct_NonexistentProduct_ExpectedToThrowArgumentNullException()
        //{
        //    var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
        //    var pictureService = new PictureService(db);
        //    var userManager = UserManagerMOQ.TestUserManager<DaysForGirlsUser>();
        //    var customerReviewService = new CustomerReviewService(userManager, db);
        //    this.adminService = new AdminService(db, pictureService, customerReviewService);
        //    await SeedSampleProducts(db);

        //    this.adminService = new AdminService(db, pictureService, customerReviewService);

        //    string pictureUrl = "New Product Picture";

        //    await Assert.ThrowsAsync<ArgumentNullException>(() => this.adminService.UploadNewPictureToProductAsync(8, pictureUrl));
        //}

        [Fact]
        public async Task AddProductToSale_WithValidData_ExpectedToAddTheProductToTheSale()
        {
            string errorMessagePrefix = "AdminService AddProductToSaleAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            var pictureService = new PictureService(db);
            var customerReviewService = new CustomerReviewService(db);
            this.adminService = new AdminService(db, pictureService, customerReviewService);
            Sale sale = new Sale
            {
                Title = "Sale One",
                EndsOn = DateTime.UtcNow.AddDays(10),
                Picture = "Sale Illustration"
            };
            db.Sales.Add(sale);
            await SeedSampleProducts(db);

            this.adminService = new AdminService(db, pictureService, customerReviewService);

            Product product = db.Products.First();
            Sale saleToAddTo = db.Sales.First();

            bool productIsAddedToSale = await this.adminService.AddProductToSaleAsync(product.Id, saleToAddTo.Id);

            Assert.True(productIsAddedToSale, errorMessagePrefix);
            Assert.True(product.IsInSale, errorMessagePrefix + " " + "IsInSale not set to true.");
            Assert.True(product.SaleId == sale.Id, errorMessagePrefix + " " + "SaleId not set correctly");
        }

        [Fact]
        public async Task AddProductToSale_WithNonexistentSale_ExpectedToThrowArgumentNullException()
        {
            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            var pictureService = new PictureService(db);
            var customerReviewService = new CustomerReviewService(db);
            this.adminService = new AdminService(db, pictureService, customerReviewService);

            await SeedSampleProducts(db);

            this.adminService = new AdminService(db, pictureService, customerReviewService);

            Product product = db.Products.First();

            await Assert.ThrowsAsync<ArgumentNullException>(() => this.adminService.AddProductToSaleAsync(product.Id, "2"));
        }

        [Fact]
        public async Task SetProductsShoppingCartIdToNull_WithExistingProducts_ExpectedToReturnTrue()
        {
            string errorMessagePrefix = "AdminService AddProductToSaleAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            var pictureService = new PictureService(db);
            var customerReviewService = new CustomerReviewService(db);
            this.adminService = new AdminService(db, pictureService, customerReviewService);

            await SeedSampleProducts(db);

            this.adminService = new AdminService(db, pictureService, customerReviewService);

            int availableItems = db.Products.First().Quantity.AvailableItems;

            List<int> productIds = db.Products.Select(p => p.Id).ToList();

            bool productsOutOfShoppingCart = await this.adminService.SetProductsCartIdToNullAsync(productIds);

            Product product = db.Products.First();

            Assert.True(productsOutOfShoppingCart, errorMessagePrefix);
            Assert.True(product.ShoppingCartId == null, errorMessagePrefix + " " + "Product ShoppingCartId not set to Null");
            Assert.True(product.Quantity.AvailableItems > availableItems, errorMessagePrefix + " " + "Product quantity is not increased back + 1");
        }

        [Fact]
        public async Task AddProductsToOrder_WithExistingProductsAndOrder_ExpectedToReturnTrue()
        {
            string errorMessagePrefix = "AdminService AddProductToSaleAsync() method does not work properly.";

            var db = DaysForGirlsDbContextInMemoryFactory.InitializeContext();
            var pictureService = new PictureService(db);
            var customerReviewService = new CustomerReviewService(db);
            this.adminService = new AdminService(db, pictureService, customerReviewService);
            Order order = new Order
            {
                IssuedOn = DateTime.UtcNow,
                User = new DaysForGirlsUser
                {
                    Address = "s",
                    Email = "ani@ani.com",
                    FirstName = "Ani",
                    LastName = "Aneva",
                    PasswordHash = "123",
                    PhoneNumber = "0888888888",
                    UserName = "Anni"
                },
                OrderStatus = "ordered"
            };
            db.Orders.Add(order);
            await SeedSampleProducts(db);

            this.adminService = new AdminService(db, pictureService, customerReviewService);

            List<int> productIds = db.Products.Select(p => p.Id).ToList();

            bool productsAreAddedToOrder = await this.adminService.SetOrderIdToProductsAsync(productIds, order.Id);
            Product product = db.Products.First();

            Assert.True(productsAreAddedToOrder, errorMessagePrefix);
            Assert.True(product.OrderId == order.Id, errorMessagePrefix + " " + "Product OrderId is not set correctly");
        }
    }
}
