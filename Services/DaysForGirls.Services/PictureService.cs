using DaysForGirls.Data;
using DaysForGirls.Data.Models;
using DaysForGirls.Services.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaysForGirls.Services
{
    public class PictureService : IPictureService
    {
        private readonly DaysForGirlsDbContext db;

        public PictureService(DaysForGirlsDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> Create(List<PictureServiceModel> pictureServiceModels, int productId)
        {
            List<Picture> allPicturesToAddToDb = new List<Picture>();

            foreach(var pSm in pictureServiceModels)
            {
                Picture picture = new Picture
                {
                    PictureUrl = pSm.PictureUrl,
                    ProductId = productId
                };

                allPicturesToAddToDb.Add(picture);
            }
            

            this.db.Pictures.AddRange(allPicturesToAddToDb);
            int result = await this.db.SaveChangesAsync();

            return result > 0;
        }

        public async Task<PictureServiceModel> GetPictureByIdAsync(int id)
        {
            var picture = await this.db.Pictures
                .SingleOrDefaultAsync(p => p.Id == id);

            PictureServiceModel pictureToReturn = new PictureServiceModel
            {
                Id = picture.Id,
                PictureUrl = picture.PictureUrl,
                ProductId = picture.ProductId
            };

            return pictureToReturn;
        }

        public IQueryable<PictureServiceModel> GetPicturesOfProductByProductId(int productId)
        {
            var pictures = this.db.Pictures
                .Where(p => p.ProductId == productId
                && p.IsDeleted == false)
                .Select(p => new PictureServiceModel
                {
                    Id = p.Id,
                    PictureUrl = p.PictureUrl,
                    ProductId = productId
                });

            return pictures;
        }

        public async Task<PictureServiceModel> GetPictureByUrl(string pictureUrl)
        {
            var picture = await this.db.Pictures
                .SingleOrDefaultAsync(p => p.PictureUrl == pictureUrl);

            var pictureToReturn = new PictureServiceModel
            {
                Id = picture.Id,
                PictureUrl = picture.PictureUrl,
                ProductId = picture.ProductId
            };

            return pictureToReturn;
        }

        public async Task<bool> UpdatePictureInfoAsync(int pictureId, int productId)
        {
            var picture = this.db.Pictures
                .SingleOrDefault(p => p.Id == pictureId);

            picture.Product = this.db.Products
                .SingleOrDefault(p => p.Id == productId);

            this.db.Pictures.Update(picture);
            int result = await this.db.SaveChangesAsync();

            return result == 1;
        }

        public async Task<bool> DeletePicturesOfDeletedProductAsync(int productId)
        {
            var picturesToDelete = await this.db.Pictures
                .Where(p => p.ProductId == productId)
                .ToListAsync();

            foreach(var picture in picturesToDelete)
            {
                picture.IsDeleted = true;
            }

            this.db.UpdateRange(picturesToDelete);
            int result = await this.db.SaveChangesAsync();

            bool picturesAreDeleted = result > 0;

            return picturesAreDeleted;
        }

        //public async Task<bool> DeletePictureWithUrl(string pictureUrl)
        //{
        //    Picture pictureToDelete = this.db.Pictures
        //        .SingleOrDefault(pic => pic.PictureUrl == pictureUrl);

        //    var product = await this.db.Products
        //        .SingleOrDefaultAsync(p => p.Id == pictureToDelete.Product.Id);

        //    product.Pictures.Remove(pictureToDelete);

        //    pictureToDelete.IsDeleted = true;

        //    this.db.UpdateRange(pictureToDelete, product);

        //    int result = await this.db.SaveChangesAsync();
        //    bool pictureIsDeleted = result > 0;

        //    return pictureIsDeleted;
        //}


        //public async Task<bool> DeletePictureWithUrl(string pictureUrl)
        //{
        //    Picture pictureToDelete = this.db.Pictures
        //        .SingleOrDefault(pic => pic.PictureUrl == pictureUrl);

        //    var product = await this.db.Products
        //        .SingleOrDefaultAsync(p => p.Id == pictureToDelete.ProductId);

        //    product.Pictures.Remove(pictureToDelete);

        //    pictureToDelete.IsDeleted = true;

        //    this.db.UpdateRange(pictureToDelete, product);

        //    int result = await this.db.SaveChangesAsync();
        //    bool pictureIsDeleted = result > 0;

        //    return pictureIsDeleted;
        //}
    }
}
