
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
            ProductType productTypeInDb = this.db.ProductTypes
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
                MainPicture = productServiceModel.MainPicture.PictureUrl,
                Description = productServiceModel.Description,
                Colour = productServiceModel.Colour,
                Size = productServiceModel.Size,
                Price = productServiceModel.Price,
                Manufacturer = manufacturerInDb,
                QuantityId = productQuantity.Id
            };

            foreach(var picture in productServiceModel.Pictures)
            {
                Picture productPicture = new Picture
                {
                    PictureUrl = picture.PictureUrl
                };
                product.Pictures.Add(productPicture);
            }

            this.db.Products.Add(product);
            int result = await db.SaveChangesAsync();

            //productServiceModel.Id = product.Id;

            return product.Id;
        }

        public async Task<ProductServiceModel> GetDetailsOfProductByIdAsync(int Id)
        {
            var productInDb = this.db.Products
                .SingleOrDefault(p => p.Id == Id);

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
                MainPicture = new PictureServiceModel
                {
                    PictureUrl = productInDb.MainPicture
                },
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
                }
            };

            List<PictureServiceModel> pCMs = new List<PictureServiceModel>();

            foreach(var pic in productInDb.Pictures)
            {
                PictureServiceModel pCM = new PictureServiceModel
                {
                    PictureUrl = pic.PictureUrl
                };

                pCMs.Add(pCM);
            }

            productToReturn.Pictures = pCMs;

            List<CustomerReviewServiceModel> cRSMs = new List<CustomerReviewServiceModel>();

            if (productInDb.Reviews.Count() > 0)
            {
                foreach(var rev in productInDb.Reviews)
                {
                    CustomerReviewServiceModel cRSM = new CustomerReviewServiceModel
                    {
                        Id = rev.Id,
                        Title = rev.Title,
                        Text = rev.Text,
                        Author = new DaysForGirlsUserServiceModel
                        {
                            FirstName = rev.Author.FirstName,
                            LastName = rev.Author.LastName
                        }
                    };

                    cRSMs.Add(cRSM);
                }
            }

            productToReturn.Reviews = cRSMs;

            await Task.Delay(0);
            return productToReturn;
        }

        public IQueryable<ProductServiceModel> DisplayAll()
        {
            var allProducts = this.db.Products
                .Where(p => p.IsDeleted == false
                && p.Quantity.AvailableItems > 0)
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
                     MainPicture = new PictureServiceModel
                     {
                         PictureUrl = p.MainPicture
                     },
                     Quantity = new QuantityServiceModel
                     {
                         AvailableItems = p.Quantity.AvailableItems
                     }
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

            return result == 1;
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
                    MainPicture = new PictureServiceModel
                    {
                        PictureUrl = p.MainPicture
                    },
                    Quantity = new QuantityServiceModel
                    {
                        AvailableItems = p.Quantity.AvailableItems
                    }
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
                    MainPicture = new PictureServiceModel
                    {
                        PictureUrl = p.MainPicture
                    },
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
                    MainPicture = new PictureServiceModel
                    {
                        PictureUrl = suit.MainPicture
                    },
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
                    }
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
                    MainPicture = new PictureServiceModel
                    {
                        PictureUrl = a.MainPicture
                    },
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
                                FirstName = accR.Author.FirstName,
                                LastName = accR.Author.LastName
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
                    MainPicture = new PictureServiceModel
                    {
                        PictureUrl = p.MainPicture
                    },
                    Quantity = new QuantityServiceModel
                    {
                        AvailableItems = p.Quantity.AvailableItems
                    }
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
                    MainPicture = new PictureServiceModel
                    {
                        PictureUrl = p.MainPicture
                    },
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
                    MainPicture = new PictureServiceModel
                    {
                        PictureUrl = p.MainPicture
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
                    MainPicture = new PictureServiceModel
                    {
                        PictureUrl = a.MainPicture
                    },
                    Colour = a.Colour,
                    Price = a.Price,
                    Manufacturer = new ManufacturerServiceModel
                    {
                        Name = a.Manufacturer.Name
                    },
                    Quantity = new QuantityServiceModel
                    {
                        AvailableItems = a.Quantity.AvailableItems
                    }
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
                    MainPicture = new PictureServiceModel
                    {
                        PictureUrl = p.MainPicture
                    },
                    Quantity = new QuantityServiceModel
                    {
                        AvailableItems = p.Quantity.AvailableItems
                    }
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
                    MainPicture = new PictureServiceModel
                    {
                        PictureUrl = p.MainPicture
                    },
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
                    MainPicture = new PictureServiceModel
                    {
                        PictureUrl = p.MainPicture
                    },
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
                    MainPicture = new PictureServiceModel
                    {
                        PictureUrl = a.MainPicture
                    },
                    Colour = a.Colour,
                    Price = a.Price,
                    Manufacturer = new ManufacturerServiceModel
                    {
                        Name = a.Manufacturer.Name
                    },
                    Quantity = new QuantityServiceModel
                    {
                        AvailableItems = a.Quantity.AvailableItems
                    }
                });

            return allOtherAccessories;
        }
    }
}
