
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaysForGirls.Data;
using DaysForGirls.Data.Models;
using DaysForGirls.Services.Models;

namespace DaysForGirls.Services
{
    public class ProductService : IProductService
    {
        private readonly DaysForGirlsDbContext db;
        private readonly IQuantityService quantityService;

        public ProductService(
            DaysForGirlsDbContext db,
            IQuantityService quantityService)
        {
            this.db = db;
            this.quantityService = quantityService;
        }

        /* instead of bool this method should actually return
        * Task<ProductServiceModel> with the
        * serviceModel Id already inserted
        */
        public async Task<int> Create(ProductServiceModel productServiceModel)
        {
            ProductType productTypeInDb = db.ProductTypes
                .SingleOrDefault(pT => pT.Name == productServiceModel.ProductType.Name);

            Category categoryInDb = this.db.Categories
                .SingleOrDefault(cat => cat.Name == productServiceModel.Category.Name);

            Manufacturer manufacturerInDb = this.db.Manufacturers
                .SingleOrDefault(man => man.Name == productServiceModel.Manufacturer.Name);

            QuantityServiceModel productQuantity = new QuantityServiceModel
            {
                AvailableItems = productServiceModel.Quantity.AvailableItems
            };

            productQuantity = await this.quantityService.Create(productQuantity);

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
                QuantityId = productQuantity.Id
            };

            foreach (PictureServiceModel picture in productServiceModel.Pictures)
            {
                Picture productPicture = new Picture
                {
                    PictureUrl = picture.PictureUrl
                };
                product.Pictures.Add(productPicture);
            }

            this.db.Products.Add(product);
            int result = await db.SaveChangesAsync();

            int productId = product.Id;

            return productId;
        }

        public async Task<ProductServiceModel> GetDetailsOfProductByIdAsync(int productId)
        {
            var productInDb = this.db.Products
                .SingleOrDefault(p => p.Id == productId);

            var productPictures = this.db.Pictures
                .Where(p => p.ProductId == productId)
                .Select(p => new PictureServiceModel
                {
                    Id = p.Id,
                    PictureUrl = p.PictureUrl,
                    ProductId = p.ProductId
                })
                .ToList();

            var productCustomerReviews = this.db.CustomerReviews
                .Where(c => c.ProductId == productId)
                .Select(c => new CustomerReviewServiceModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    Text = c.Text,
                    Author = new DaysForGirlsUserServiceModel
                    {
                        UserName = c.Author.UserName
                    }
                })
                .ToList();

            var productTypeOfProduct = this.db.ProductTypes
                .SingleOrDefault(pT => pT.Id == productInDb.ProductTypeId);

            var categoryOfProduct = this.db.Categories
                .SingleOrDefault(c => c.Id == productInDb.CategoryId);

            var manufacturerOfProduct = this.db.Manufacturers
                .SingleOrDefault(m => m.Id == productInDb.ManufacturerId);

            var quantityOfProduct = this.db.Quantities
                .SingleOrDefault(q => q.Id == productInDb.QuantityId);

            productInDb.Category = categoryOfProduct;
            productInDb.ProductType = productTypeOfProduct;
            productInDb.Manufacturer = manufacturerOfProduct;
            productInDb.Quantity = quantityOfProduct;

            ProductServiceModel productToReturn = new ProductServiceModel
            {
                Id = productInDb.Id,
                Name = productInDb.Name,
                ProductType = new ProductTypeServiceModel
                {
                    Name = productInDb.ProductType.Name
                },
                Category = new CategoryServiceModel
                {
                    Name = productInDb.Category.Name
                },
                Description = productInDb.Description,
                Colour = productInDb.Colour,
                Size = productInDb.Size,
                Price = productInDb.Price,
                Manufacturer = new ManufacturerServiceModel
                {
                    Name = productInDb.Manufacturer.Name
                },
                Quantity = new QuantityServiceModel
                {
                    AvailableItems = productInDb.Quantity.AvailableItems
                },
                Pictures = productPictures,
                Reviews = productCustomerReviews
            };

            await Task.Delay(0);
            return productToReturn;
        }

        public IQueryable<ProductServiceModel> DisplayAll()
        {
            var allProducts = this.db.Products
                .Where(p => p.IsDeleted == false)
                .Select(p => new ProductServiceModel
                 {
                     Id = p.Id,
                     Name = p.Name,
                     Price = p.Price,
                     Category = new CategoryServiceModel
                     {
                         Name = p.Category.Name
                     },
                     ProductType = new ProductTypeServiceModel
                     {
                         Name = p.ProductType.Name
                     },
                     Quantity = new QuantityServiceModel
                     {
                         AvailableItems = p.Quantity.AvailableItems
                     },
                     Pictures = p.Pictures
                        .Select(pU => new PictureServiceModel
                        {
                            PictureUrl = pU.PictureUrl
                        }).ToList()
                 });

            return allProducts;
        }

        public async Task<bool> GetProductToDeleteById(int id)
        {
            var productToDelete = this.db.Products
                .SingleOrDefault(product => product.Id == id);

            productToDelete.IsDeleted = true;

            this.db.Update(productToDelete);
            int result = await this.db.SaveChangesAsync();

            return result > 0;
        }

        public IQueryable<ProductServiceModel> AllWeddingProducts()
        {
            var allWeddingProducts = this.db.Products
                .Where(p => p.Category.Name == "Wedding")
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
                    Price = p.Price,
                    Quantity = new QuantityServiceModel
                    {
                        AvailableItems = p.Quantity.AvailableItems
                    },
                    Pictures = p.Pictures
                        .Select(pU => new PictureServiceModel
                        {
                            PictureUrl = pU.PictureUrl
                        }).ToList()
                });

            return allWeddingProducts;
        }

        public IQueryable<ProductServiceModel> AllWeddingDresses()
        {
            var allWeddingDresses = this.db.Products
                .Where(p => p.Category.Name == "Wedding"
                && p.ProductType.Name == "Dress")
                .Select(p => new ProductServiceModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    ProductType = new ProductTypeServiceModel
                    {
                        Name = p.ProductType.Name
                    },
                    Category = new CategoryServiceModel
                    {
                        Name = p.Category.Name
                    },
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
                    },
                    Pictures = p.Pictures
                        .Select(pU => new PictureServiceModel
                        {
                            PictureUrl = pU.PictureUrl
                        }).ToList()
                });

            return allWeddingDresses;
        }

        public IQueryable<ProductServiceModel> AllWeddingSuits()
        {
            var allWeddingSuits = this.db.Products
                .Where(p => p.Category.Name == "Wedding"
                && p.ProductType.Name == "Suit")
                .Select(suit => new ProductServiceModel
                {
                    Id = suit.Id,
                    Name = suit.Name,
                    ProductType = new ProductTypeServiceModel
                    {
                        Name = suit.ProductType.Name
                    },
                    Category = new CategoryServiceModel
                    {
                        Name = suit.Category.Name
                    },
                    Description = suit.Description,
                    Colour = suit.Colour,
                    Size = suit.Size,
                    Price = suit.Price,
                    Manufacturer = new ManufacturerServiceModel
                    {
                        Name = suit.Manufacturer.Name
                    },
                    Quantity = new QuantityServiceModel
                    {
                        AvailableItems = suit.Quantity.AvailableItems
                    },
                    Pictures = suit.Pictures
                        .Select(pU => new PictureServiceModel
                        {
                            PictureUrl = pU.PictureUrl
                        }).ToList()
                });

            return allWeddingSuits;
        }

        public IQueryable<ProductServiceModel> AllWeddingAccessories()
        {
            var allWeddingAccessories = this.db.Products
                .Where(a => a.Category.Name == "Wedding"
                && a.ProductType.Name == "Accessory")
                .Select(a => new ProductServiceModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Category = new CategoryServiceModel
                    {
                        Name = a.Category.Name
                    },
                    Description = a.Description,
                    Colour = a.Colour,
                    Price = a.Price,
                    Manufacturer = new ManufacturerServiceModel
                    {
                        Name = a.Manufacturer.Name
                    },
                    Quantity = new QuantityServiceModel
                    {
                        AvailableItems = a.Quantity.AvailableItems
                    },
                    Pictures = a.Pictures
                        .Select(pU => new PictureServiceModel
                        {
                            PictureUrl = pU.PictureUrl
                        }).ToList(),
                    Reviews = a.Reviews
                        .Select(accR => new CustomerReviewServiceModel
                        {
                            Id = accR.Id,
                            Title = accR.Title,
                            Text = accR.Text,
                            Author = new DaysForGirlsUserServiceModel
                            {
                                Username = accR.Author.UserName
                            }
                        }).ToList()
                });

            return allWeddingAccessories;
        }

        public IQueryable<ProductServiceModel> AllPromProducts()
        {
            var allPromProducts = this.db.Products
                .Where(p => p.Category.Name == "Prom")
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
                    Price = p.Price,
                    Quantity = new QuantityServiceModel
                    {
                        AvailableItems = p.Quantity.AvailableItems
                    },
                    Pictures = p.Pictures
                        .Select(pU => new PictureServiceModel
                        {
                            PictureUrl = pU.PictureUrl
                        }).ToList(),
                    Reviews = p.Reviews
                        .Select(accR => new CustomerReviewServiceModel
                        {
                            Id = accR.Id,
                            Title = accR.Title,
                            Text = accR.Text,
                            Author = new DaysForGirlsUserServiceModel
                            {
                                Username = accR.Author.UserName
                            }
                        }).ToList()
                });

            return allPromProducts;
        }

        public IQueryable<ProductServiceModel> AllPromDresses()
        {
            var allPromDresses = this.db.Products
                .Where(p => p.Category.Name == "Prom"
                && p.ProductType.Name == "Dress")
                .Select(p => new ProductServiceModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    ProductType = new ProductTypeServiceModel
                    {
                        Name = p.ProductType.Name
                    },
                    Category = new CategoryServiceModel
                    {
                        Name = p.Category.Name
                    },
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
                    },
                    Reviews = p.Reviews
                        .Select(accR => new CustomerReviewServiceModel
                        {
                            Id = accR.Id,
                            Title = accR.Title,
                            Text = accR.Text,
                            Author = new DaysForGirlsUserServiceModel
                            {
                                Username = accR.Author.UserName
                            }
                        }).ToList(),
                    Pictures = p.Pictures
                        .Select(pU => new PictureServiceModel
                        {
                            PictureUrl = pU.PictureUrl
                        }).ToList()
                });

            return allPromDresses;
        }

        public IQueryable<ProductServiceModel> AllPromSuits()
        {
            var allPromSuits = this.db.Products
                .Where(p => p.Category.Name == "Prom"
                && p.ProductType.Name == "Suit")
                .Select(p => new ProductServiceModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ProductType = new ProductTypeServiceModel
                    {
                        Name = p.ProductType.Name
                    },
                    Quantity = new QuantityServiceModel
                    {
                        AvailableItems = p.Quantity.AvailableItems
                    }
                });

            return allPromSuits;
        }

        public IQueryable<ProductServiceModel> AllPromAccessories()
        {
            var allPromAccessories = this.db.Products
                .Where(p => p.Category.Name == "Prom"
                && p.ProductType.Name == "Accessory")
                .Select(a => new ProductServiceModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Category = new CategoryServiceModel
                    {
                        Name = a.Category.Name
                    },
                    Description = a.Description,
                    Colour = a.Colour,
                    Price = a.Price,
                    Manufacturer = new ManufacturerServiceModel
                    {
                        Name = a.Manufacturer.Name
                    },
                    Quantity = new QuantityServiceModel
                    {
                        AvailableItems = a.Quantity.AvailableItems
                    },
                    Reviews = a.Reviews
                        .Select(accR => new CustomerReviewServiceModel
                        {
                            Id = accR.Id,
                            Title = accR.Title,
                            Text = accR.Text,
                            Author = new DaysForGirlsUserServiceModel
                            {
                                Username = accR.Author.UserName
                            }
                        }).ToList(),
                    Pictures = a.Pictures
                        .Select(pU => new PictureServiceModel
                        {
                            PictureUrl = pU.PictureUrl
                        }).ToList()
                });

            return allPromAccessories;
        }

        public IQueryable<ProductServiceModel> AllOtherProducts()
        {
            var allOtherProducts = this.db.Products
                .Where(p => p.Category.Name == "Other")
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
                    Price = p.Price,
                    Quantity = new QuantityServiceModel
                    {
                        AvailableItems = p.Quantity.AvailableItems
                    },
                    Reviews = p.Reviews
                        .Select(accR => new CustomerReviewServiceModel
                        {
                            Id = accR.Id,
                            Title = accR.Title,
                            Text = accR.Text,
                            Author = new DaysForGirlsUserServiceModel
                            {
                                Username = accR.Author.UserName
                            }
                        }).ToList(),
                    Pictures = p.Pictures
                        .Select(pU => new PictureServiceModel
                        {
                            PictureUrl = pU.PictureUrl
                        }).ToList()
                });

            return allOtherProducts;
        }

        public IQueryable<ProductServiceModel> AllOtherDresses()
        {
            var allOtherDresses = this.db.Products
                .Where(p => p.Category.Name == "Other"
                && p.ProductType.Name == "Dress")
                .Select(p => new ProductServiceModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    ProductType = new ProductTypeServiceModel
                    {
                        Name = p.ProductType.Name
                    },
                    Category = new CategoryServiceModel
                    {
                        Name = p.Category.Name
                    },
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
                    },
                    Reviews = p.Reviews
                        .Select(accR => new CustomerReviewServiceModel
                        {
                            Id = accR.Id,
                            Title = accR.Title,
                            Text = accR.Text,
                            Author = new DaysForGirlsUserServiceModel
                            {
                                Username = accR.Author.UserName
                            }
                        }).ToList(),
                    Pictures = p.Pictures
                        .Select(pU => new PictureServiceModel
                        {
                            PictureUrl = pU.PictureUrl
                        }).ToList()
                });

            return allOtherDresses;
        }

        public IQueryable<ProductServiceModel> AllOtherSuits()
        {
            var allOtherSuits = this.db.Products
                .Where(p => p.Category.Name == "Other"
                && p.ProductType.Name == "Suit")
                .Select(p => new ProductServiceModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    ProductType = new ProductTypeServiceModel
                    {
                        Name = p.ProductType.Name
                    },
                    Category = new CategoryServiceModel
                    {
                        Name = p.Category.Name
                    },
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
                    },
                    Reviews = p.Reviews
                        .Select(accR => new CustomerReviewServiceModel
                        {
                            Id = accR.Id,
                            Title = accR.Title,
                            Text = accR.Text,
                            Author = new DaysForGirlsUserServiceModel
                            {
                                Username = accR.Author.UserName
                            }
                        }).ToList(),
                    Pictures = p.Pictures
                        .Select(pU => new PictureServiceModel
                        {
                            PictureUrl = pU.PictureUrl
                        }).ToList()
                });

            return allOtherSuits;
        }

        public IQueryable<ProductServiceModel> AllOtherAccessories()
        {
            var allOtherAccessories = this.db.Products
                .Where(a => a.Category.Name == "Other"
                && a.ProductType.Name == "Accessory")
                .Select(a => new ProductServiceModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Category = new CategoryServiceModel
                    {
                        Name = a.Category.Name
                    },
                    Description = a.Description,
                    Colour = a.Colour,
                    Price = a.Price,
                    Manufacturer = new ManufacturerServiceModel
                    {
                        Name = a.Manufacturer.Name
                    },
                    Quantity = new QuantityServiceModel
                    {
                        AvailableItems = a.Quantity.AvailableItems
                    },
                    Reviews = a.Reviews
                        .Select(accR => new CustomerReviewServiceModel
                        {
                            Id = accR.Id,
                            Title = accR.Title,
                            Text = accR.Text,
                            Author = new DaysForGirlsUserServiceModel
                            {
                                Username = accR.Author.UserName
                            }
                        }).ToList(),
                    Pictures = a.Pictures
                        .Select(pU => new PictureServiceModel
                        {
                            PictureUrl = pU.PictureUrl
                        }).ToList()
                });

            return allOtherAccessories;
        }

        public async Task<bool> AddReviewToProductByProductIdAsync(int productId, int reviewId)
        {
            var product = this.db.Products
                .SingleOrDefault(p => p.Id == productId);

            var review = this.db.CustomerReviews
                .SingleOrDefault(r => r.Id == reviewId);

            product.Reviews.Add(review);
            this.db.Products.Update(product);
            int result = await this.db.SaveChangesAsync();

            return result > 0;
        }

        public IQueryable<ProductServiceModel> GetAllProductsByCategoryAndType(string productType, string category)
        {
            var allProductsOfCategoryAndType = this.db.Products
                .Where(p => p.Category.Name == category
                && p.ProductType.Name == productType
                && p.IsDeleted == false)
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
                        .Select(pp => new PictureServiceModel
                        {
                            Id = pp.Id,
                            PictureUrl = pp.PictureUrl
                        }).ToList(),
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
                    },
                    Reviews = p.Reviews
                        .Select(pR => new CustomerReviewServiceModel
                        {
                            Id = pR.Id,
                            Title = pR.Title,
                            Text = pR.Text,
                            Author = new DaysForGirlsUserServiceModel
                            {
                                Username = pR.Author.UserName
                            }
                        }).ToList()
                });

            return allProductsOfCategoryAndType;
        }
    }
}
