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

        public ProductController(
            IProductService productService,
            IProductTypeService productTypeService,
            ICategoryService categoryService,
            IManufacturerService manufacturerService,
            ICloudinaryService cloudinaryService,
            IPictureService pictureService)
        {
            this.productService = productService;
            this.productTypeService = productTypeService;
            this.categoryService = categoryService;
            this.manufacturerService = manufacturerService;
            this.cloudinaryService = cloudinaryService;
            this.pictureService = pictureService;
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
                .DisplayAll()
                .ToListAsync();

            this.ViewData["manufacturers"] = allManufacturers
                .Select(m => new ProductCreateManufacturerViewModel
                {
                    Name = m.Name
                })
                .OrderBy(man => man.Name)
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
            return Redirect("/Products/All");
        }

        [HttpGet("/Administration/Product/All")]
        public async Task<IActionResult> All()
        {
            var allProducts = await this.productService
                .DisplayAll().ToListAsync();

            List<ProductDisplayAllViewModel> productsToReturn = new List<ProductDisplayAllViewModel>();

            foreach(var product in allProducts)
            {
                ProductDisplayAllViewModel pDAVM = new ProductDisplayAllViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Category = product.Category.Name,
                    ProductType = product.ProductType.Name,
                    Price = product.Price,
                    Quantity = product.Quantity.AvailableItems
                };
                List<string> productPictures = new List<string>();
                foreach(var pic in product.Pictures)
                {
                    string Url = pic.PictureUrl;
                    productPictures.Add(Url);
                }
                pDAVM.Pictures = productPictures;
                productsToReturn.Add(pDAVM);
            }

            return View(productsToReturn);
        }

        [HttpGet("/Administration/Product/Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var productInDb = await this.productService
                .GetDetailsOfProductByIdAsync(id);

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
                AvailableQuantity = productInDb.Quantity.AvailableItems
            };

            List<PictureDisplayAllViewModel> pictures = new List<PictureDisplayAllViewModel>();

            foreach(var pic in productInDb.Pictures)
            {
                PictureDisplayAllViewModel pDAVM = new PictureDisplayAllViewModel
                {
                    ImageUrl = pic.PictureUrl
                };

                pictures.Add(pDAVM);
            }

            productToDisplay.Pictures = pictures;

            List<CustomerReviewAllViewModel> reviews = new List<CustomerReviewAllViewModel>();

            foreach(var rev in productInDb.Reviews)
            {
                CustomerReviewAllViewModel cRAVM = new CustomerReviewAllViewModel
                {
                    Id = rev.Id,
                    Title = rev.Title,
                    Text = rev.Text,
                    DateCreated = rev.CreatedOn.ToString(),
                    Author = rev.Author.Username
                };

                reviews.Add(cRAVM);
            }

            productToDisplay.Reviews = reviews;

            return View(productToDisplay);
        }
    }
}
