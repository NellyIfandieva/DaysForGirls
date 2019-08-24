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
        private readonly IAdminService adminService;
        private readonly IProductTypeService productTypeService;
        private readonly ICategoryService categoryService;
        private readonly IManufacturerService manufacturerService;
        private readonly ICloudinaryService cloudinaryService;
        private readonly IPictureService pictureService;
        private readonly ISaleService saleService;

        public ProductController(
            IAdminService adminService,
            IProductTypeService productTypeService,
            ICategoryService categoryService,
            IManufacturerService manufacturerService,
            ICloudinaryService cloudinaryService,
            IPictureService pictureService,
            ISaleService saleService)
        {
            this.adminService = adminService;
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateInputModel model)
        {
            if(ModelState.IsValid == false)
            {
                return this.View(model);
            }

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
            string saleId = null;
            if(model.SaleTitle != null)
            {
                var sale = await this.saleService.GetSaleByTitleAsync(model.SaleTitle);
                saleId = sale.Id;
            }

            List<string> imageUrls = new List<string>();

            int imageNameExtension = 1;

            foreach(var iFormFile in model.Pictures)
            {
                string imageUrl = await this.cloudinaryService.UploadPictureForProductAsync(
                iFormFile, model.Name + "_" + imageNameExtension++);

                imageUrls.Add(imageUrl);
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
                },
                SaleId = saleId
            };

            productServiceModel.Pictures = imageUrls.Select(image => new PictureServiceModel
            {
                PictureUrl = image
            })
            .ToList();

            int productId = await this.adminService.CreateAsync(productServiceModel);

            if (model.SaleTitle != null)
            {
                bool saleAddedProduct = await this.saleService.AddProductToSaleAsync(productServiceModel.SaleId, productId);
                bool productIsInSale = await this.adminService.AddProductToSaleAsync(productId, productServiceModel.SaleId);
            }

            return Redirect("/Administration/Product/All");
        }

        [HttpGet("/Administration/Product/All")]
        public async Task<IActionResult> All()
        {
            var allProducts = await this.adminService
                .DisplayAll()
                .Select(product => new AdminDisplayAllViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Category = product.Category.Name,
                    Picture = product.Picture.PictureUrl,
                    Price = product.Price,
                    AvailableItems = product.AvailableItems,
                    Manufacturer = product.Manufacturer.Name,
                    IsDeleted = product.IsDeleted,
                    IsInSale = product.IsInSale,
                    SaleId = product.SaleId,
                    ShoppingCartId = product.ShoppingCartId,
                    OrderId = product.OrderId
                }).ToListAsync();

            if(allProducts == null)
            {
                return NotFound();
            }

            return View(allProducts);
        }

        [HttpGet("/Administration/Product/Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if(id <= 0)
            {
                return BadRequest();
            }

            var productInDb = await this.adminService
                .GetProductByIdAsync(id);

            if(productInDb == null)
            {
                return NotFound();
            }

            ProductDetailsViewModel productToDisplay = new ProductDetailsViewModel
            {
                Id = productInDb.Id,
                Name = productInDb.Name,
                Category = productInDb.Category.Name,
                ProductType = productInDb.ProductType.Name,
                Colour = productInDb.Colour,
                Size = productInDb.Size,
                Description = productInDb.Description,
                Manufacturer = productInDb.Manufacturer.Name,
                Price = productInDb.Price,
                AvailableQuantity = productInDb.Quantity.AvailableItems,
                Pictures = productInDb.Pictures
                    .Select(p => new PictureDisplayAllViewModel
                    {
                        Id = p.Id,
                        ImageUrl = p.PictureUrl,
                        ProductId = productInDb.Id
                    }).ToList(),
                IsDeleted = productInDb.IsDeleted,
                Reviews = productInDb.Reviews
                    .Select(pR => new CustomerReviewAllViewModel
                    {
                        Id = pR.Id,
                        Title = pR.Title,
                        Text = pR.Text,
                        DateCreated = pR.CreatedOn,
                        Author = pR.AuthorUsername,
                    })
                    .ToList(),
                ShoppingCartId = productInDb.ShoppingCartId,
                OrderId = productInDb.OrderId
            };

            return View(productToDisplay);
        }

        [HttpGet("/Administration/Product/Edit/{productId}")]
        public async Task<IActionResult> Edit(int productId)
        {
            if(productId <= 0)
            {
                return BadRequest();
            }

            var productWithId = 
                await this.adminService.GetProductByIdAsync(productId);

            if(productWithId == null)
            {
                return NotFound();
            }

            var productToEdit = new ProductEditInputModel
            {
                ProductId = productWithId.Id,
                Name = productWithId.Name,
                Description = productWithId.Description,
                Colour = productWithId.Colour,
                Size = productWithId.Size,
                Price = productWithId.Price,
                Quantity = productWithId.Quantity.AvailableItems
            };

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

            var allActiveSales = await this.saleService
                .DisplayAll().ToListAsync();

            this.ViewData["sales"] = allActiveSales
                .Select(s => new ProductCreateSaleViewModel
                {
                    Title = s.Title
                })
                .OrderBy(s => s.Title)
                .ToList();

            return this.View(productToEdit);
        }

        [HttpPost("/Administration/Product/Edit/{productId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int productId, ProductEditInputModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                var productTypes = await this.productTypeService.DisplayAll().ToListAsync();

                this.ViewData["types"] = productTypes.Select(pT => new ProductCreateProductTypeViewModel
                {
                    Name = pT.Name
                })
                .ToList(); ;

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

                var allActiveSales = await this.saleService
                .DisplayAll().ToListAsync();

                this.ViewData["sales"] = allActiveSales
                    .Select(s => new ProductCreateSaleViewModel
                    {
                        Title = s.Title
                    })
                    .OrderBy(s => s.Title)
                    .ToList();

                return this.View(model);
            }

            string saleId = null;

            if (model.SaleTitle != null)
            {
                var sale = await this.saleService.GetSaleByTitleAsync(model.SaleTitle);
                saleId = sale.Id;
            }

            List<string> imageUrls = new List<string>();

            int imageNameExtension = 1;

            foreach (var iFormFile in model.Pictures)
            {
                string imageUrl = await this.cloudinaryService.UploadPictureForProductAsync(
                iFormFile, model.Name + "_" + imageNameExtension++);

                imageUrls.Add(imageUrl);
            }

            var productToEdit = new ProductServiceModel
            {
                Id = productId,
                Name = model.Name,
                Category = new CategoryServiceModel { Name = model.Category },
                ProductType = new ProductTypeServiceModel { Name = model.ProductType },
                Description = model.Description,
                Colour = model.Colour,
                Size = model.Size,
                Price = model.Price,
                Manufacturer = new ManufacturerServiceModel { Name = model.Manufacturer },
                Quantity = new QuantityServiceModel { AvailableItems = model.Quantity }
            };

            productToEdit.Pictures = imageUrls.Select(image => new PictureServiceModel
            {
                PictureUrl = image
            }).ToList();

            bool productIsEdited = await this.adminService.EditAsync(productToEdit);

            if (model.SaleTitle != null)
            {
                bool saleAddedProduct = await this.saleService.AddProductToSaleAsync(saleId, productToEdit.Id);
                bool productIsInSale = await this.adminService.AddProductToSaleAsync(productToEdit.Id, saleId);
            }

            return this.Redirect("/Administration/Product/Details/" + productId);
        }

        //public async Task<IActionResult> DeletePicture(string pictureUrl)
        //{
        //    bool pictureIsDeleted = await this.pictureService
        //        .DeletePictureWithUrl(pictureUrl);

        //    if(pictureIsDeleted == false)
        //    {
        //        return View("Administration/Product/Edit/{productId}");
        //    }

        //    return View("Administration/Product/Details/{productId}");
        //}

        //public async Task<IActionResult> UploadNewPicture(ProductEditInputModel model)
        //{
        //    int productId = model.ProductId;

        //    Guid name = new Guid();

        //    //string imageUrl = await this.cloudinaryService.UploadPictureForProductAsync(
        //    //    model.NewPicture, name.ToString());

        //    //bool imageIsAdded = await this.adminService.UploadNewPictureToProductAsync(productId, imageUrl);

        //    return Redirect("/Administration/Product/Details/{productId}");
        //}

        [HttpDelete("/Administration/Product/Delete/{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            bool isDeleted = await this.adminService.DeleteProductByIdAsync(productId);

            return Redirect("/Administration/Product/All");
        }
    }
}
