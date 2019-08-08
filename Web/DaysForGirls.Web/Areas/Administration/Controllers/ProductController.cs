using DaysForGirls.Services;
using DaysForGirls.Services.Models;
using DaysForGirls.Web.InputModels;
using DaysForGirls.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaysForGirls.Web.Areas.Administration.Controllers
{
    public class ProductController : AdminController
    {
        private readonly IProductService productService;
        private readonly IProductTypeService productTypeService;
        private readonly ICategoryService categoryService;
        private readonly IManufacturerService manufacturerService;
        private readonly ICloudinaryService cloudinaryService;
        private readonly IPictureService pictureService;
        private readonly ISaleService saleService;

        public ProductController(
            IProductService productService,
            IProductTypeService productTypeService,
            ICategoryService categoryService,
            IManufacturerService manufacturerService,
            ICloudinaryService cloudinaryService,
            IPictureService pictureService,
            ISaleService saleService)
        {
            this.productService = productService;
            this.productTypeService = productTypeService;
            this.categoryService = categoryService;
            this.manufacturerService = manufacturerService;
            this.cloudinaryService = cloudinaryService;
            this.pictureService = pictureService;
            this.saleService = saleService;
        }

        [HttpGet("/Administration/Product/Create")]
        public async Task<IActionResult> Create()
        {
            var allProductTypes = await this.productTypeService
                .DisplayAll()
                .ToListAsync();

            this.ViewData["productTypes"] = allProductTypes
                .Select(pT => new ProductCreateProductTypeViewModel
                {
                    Name = pT.Name
                })
                .OrderBy(t => t.Name)
                .ToList();

            var allCategories = await this.categoryService
                .DisplayAll()
                .ToListAsync();

            this.ViewData["categories"] = allCategories
                .Select(c => new ProductCreateCategoryViewModel
                {
                    Name = c.Name
                })
                .OrderBy(cat => cat.Name)
                .ToList();

            var allManufacturers = await this.manufacturerService
                .DisplayAll().ToListAsync();

            this.ViewData["manufacturers"] = allManufacturers
                .Select(m => new ProductCreateManufacturerViewModel
                {
                    Name = m.Name
                })
                .OrderBy(man => man.Name)
                .ToList();

            var allActiveSales = await this.saleService
                .DisplayAll().ToListAsync();

            this.ViewData["sales"] = allActiveSales
                .Select(s => new ProductCreateSaleViewModel
                {
                    Title = s.Title
                })
                .OrderBy(s => s.Title)
                .ToList();

            return View();
        }

        [HttpPost("/Administration/Product/Create")]
        public async Task<IActionResult> Create(ProductCreateInputModel model)
        {
            if(ModelState.IsValid == false)
            {
                return this.View(model);
            }
            //TODO Re-do the productType, category and Manuf
            //Add picture to pSM
            //TODO ModelState Validation

            var productType = new ProductTypeServiceModel
            {
                Name = model.ProductType
            };

            var category = new CategoryServiceModel
            {
                Name = model.Category
            };

            var manufacturer = new ManufacturerServiceModel
            {
                Name = model.Manufacturer
            };

            List<string> imageUrls = new List<string>();
            List<PictureServiceModel> pictureServiceModels = new List<PictureServiceModel>();

            int imageNameExtension = 1;

            //string mainImageUrl = await this.cloudinaryService
            //    .UploadPictureForProductAsync(model.MainPicture, model.Name + "_Main");
            //PictureServiceModel picServModel = new PictureServiceModel { PictureUrl = mainImageUrl };
            //pictureServiceModels.Add(picServModel);

            //imageUrls.Add(mainImageUrl);

            foreach(var iFormFile in model.Pictures)
            {
                string imageUrl = await this.cloudinaryService.UploadPictureForProductAsync(
                iFormFile, model.Name + "_" + imageNameExtension++);

                imageUrls.Add(imageUrl);

                //PictureServiceModel pictureServiceModel = new PictureServiceModel { PictureUrl = imageUrl};
                //pictureServiceModels.Add(pictureServiceModel);
            }

            ProductServiceModel productServiceModel = new ProductServiceModel
            {
                Name = model.Name,
                ProductType = productType,
                Category = category,
                Description = model.Description,
                Colour = model.Colour,
                Size = model.Size,
                Price = model.Price,
                Manufacturer = manufacturer,
                Quantity = new QuantityServiceModel
                {
                    AvailableItems = model.Quantity
                }
            };

            //productServiceModel.MainPicture = new PictureServiceModel
            //{
            //    PictureUrl = imageUrls[0]
            //};

            //List<PictureServiceModel> pCMs = new List<PictureServiceModel>();

            for(int i = 0; i < imageUrls.Count; i++)
            {
                PictureServiceModel pSM = new PictureServiceModel
                {
                    PictureUrl = imageUrls[i]
                };

                pictureServiceModels.Add(pSM);
            }

            productServiceModel.Pictures = pictureServiceModels;

            int productId = await this.productService.Create(productServiceModel);

            //bool isUpdated = await this.pictureService.UpdatePictureInfoAsync(pictureInDbId, productId);
            //when i'm done re-doing the service (and IService)
            //productServiceModel = await this.productService.Create(productServiceModel);
            //bool picturesAddedToDb = await this.pictureService.Create(pictureServiceModels, productId);
            return Redirect("/Administration/Product/All");
        }

        [HttpGet("/Administration/Product/All")]
        public async Task<IActionResult> All()
        {
            var allProducts = await this.productService
                .DisplayAll()
                .Select(product => new ProductDisplayAllViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Category = product.Category.Name,
                    ProductType = product.ProductType.Name,
                    Picture = product.Pictures[0].PictureUrl,
                    Price = product.Price,
                    Quantity = product.Quantity.AvailableItems,
                    IsDeleted = product.IsDeleted,
                    IsInSale = product.IsInSale
                }).ToListAsync();

            return View(allProducts);
        }

        [HttpGet("/Administration/Product/Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            ProductServiceModel productInDb = await this.productService
                .GetProductDetailsById(id);

            ProductDetailsViewModel productToDisplay = new ProductDetailsViewModel
            {
                Id = productInDb.Id,
                Name = productInDb.Name,
                Category = productInDb.Category.Name,
                ProductType = productInDb.ProductType.Name,
                Colour = productInDb.Colour,
                Size = productInDb.Size,
                Description = productInDb.Description,
                Pictures = productInDb.Pictures
                    .Select(p => new PictureDisplayAllViewModel
                    {
                        Id = p.Id,
                        ImageUrl = p.PictureUrl
                    }).ToList(),
                Manufacturer = productInDb.Manufacturer.Name,
                Price = productInDb.Price,
                AvailableQuantity = productInDb.Quantity.AvailableItems,
                Reviews = productInDb.Reviews
                    .Select(pR => new CustomerReviewAllViewModel
                    {
                        Id = pR.Id,
                        Title = pR.Title,
                        Text = pR.Text,
                        DateCreated = pR.CreatedOn,
                        Author = pR.Author.Username
                    })
                    .ToList()
            };

            return View(productToDisplay);
        }

        [HttpGet("/Administration/Product/Edit/{productId}")]
        public async Task<IActionResult> Edit(int productId)
        {
            var productWithId = 
                await this.productService.GetProductDetailsById(productId);

            var currentProductPictures = await this.pictureService
                .GetPicturesOfProductByProductId(productId).ToListAsync();

            var currentProductPicturesToStrings = new List<string>();

            foreach(var pic in currentProductPictures)
            {
                string url = pic.PictureUrl;
                currentProductPicturesToStrings.Add(pic.PictureUrl);
            }

            var productToEdit = new ProductEditInputModel
            {
                Id = productWithId.Id,
                Name = productWithId.Name,
                Description = productWithId.Description,
                Colour = productWithId.Colour,
                Size = productWithId.Size,
                CurrentPictures = currentProductPicturesToStrings,
                Price = productWithId.Price,
                Quantity = productWithId.Quantity.AvailableItems
            };

            if (productToEdit == null)
            {
                // TODO: Error Handling
                return this.Redirect("/");
            }

            var allProductTypes = 
                await this.productTypeService.DisplayAll()
                .ToListAsync();

            this.ViewData["types"] = allProductTypes
                .Select(productType => new ProductCreateProductTypeViewModel
                {
                    Name = productType.Name
                }).ToList();

            var allCategories = await this.categoryService.DisplayAll()
                .ToListAsync();

            this.ViewData["categories"] = allCategories
                .Select(category => new ProductCreateCategoryViewModel
                {
                    Name = category.Name
                }).ToList();

            var allManufacturers = await this.manufacturerService.DisplayAll()
                .ToListAsync();

            this.ViewData["manufacturers"] = allManufacturers
                .Select(manufacturer => new ProductCreateManufacturerViewModel
                {
                    Name = manufacturer.Name
                })
                .ToList();

            return this.View(productToEdit);
        }

        [HttpPost("/Administration/Product/Edit/{productId}")]
        public async Task<IActionResult> Edit(int productId, ProductEditInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                var productTypes = await this.productTypeService.DisplayAll().ToListAsync();

                this.ViewData["types"] = productTypes.Select(pT => new ProductCreateProductTypeViewModel
                {
                    Name = pT.Name
                }).ToList(); ;

                var categories = await this.categoryService.DisplayAll().ToListAsync();

                this.ViewData["categories"] = categories.Select(c => new ProductCreateCategoryViewModel
                {
                    Name = c.Name
                });

                var manufacturers = await this.manufacturerService.DisplayAll().ToListAsync();

                this.ViewData["manufacturers"] = manufacturers.Select(m => new ProductCreateManufacturerViewModel
                {
                    Name = m.Name
                });

                return this.View(model);
            }

            var productWithEdits = new ProductServiceModel
            {
                Id = productId,
                Name = model.Name,
                Category = new CategoryServiceModel
                {
                    Name = model.Category
                },
                ProductType = new ProductTypeServiceModel
                {
                    Name = model.ProductType
                },
                Description = model.Description,
                Colour = model.Colour,
                Size = model.Size,
                Price = model.Price,
                Manufacturer = new ManufacturerServiceModel
                {
                    Name = model.Manufacturer
                },
                Quantity = new QuantityServiceModel
                {
                    AvailableItems = model.Quantity
                }
            };

            bool productIsEditedInDb = await this.productService.Edit(productId, productWithEdits);

            return this.Redirect("/");
        }

        public async Task<IActionResult> DeletePicture(string pictureUrl)
        {
            bool pictureIsDeleted = await this.productService
                .DeletePictureWithUrl(pictureUrl);

            if(pictureIsDeleted)
            {
                return View("Administration/Product/Edit/{productId}");
            }

            return View("Administration/Product/All");
        }

        public async Task<IActionResult> UploadNewPicture(ProductEditInputModel model)
        {
            int productId = model.Id;

            Guid name = new Guid();

            string imageUrl = await this.cloudinaryService.UploadPictureForProductAsync(
                model.NewPicture, name.ToString());

            bool imageIsAdded = await this.productService.UploadNewPictureToProduct(productId, imageUrl);

            return Redirect("/Administration/Product/Edit/{productId}");
        }

        [HttpGet("/Administration/Product/Delete/{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            bool isDeleted = await this.productService.DeleteProductById(productId);

            return Redirect("/Administration/Product/All");
        }
    }
}
