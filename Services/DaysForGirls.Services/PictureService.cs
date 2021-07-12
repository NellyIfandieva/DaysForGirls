namespace DaysForGirls.Services
{
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class PictureService : IPictureService
    {
        private readonly DaysForGirlsDbContext db;

        public PictureService(DaysForGirlsDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> Create(List<PictureServiceModel> pictureServiceModels, int productId)
        {
            if(productId <= 0)
            {
                return false;
            }

            var allPicturesToAddToDb = new List<Picture>();

            foreach (var pSm in pictureServiceModels)
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

            var picturesAreCreated = result > 0;

            return picturesAreCreated;
        }

        public async Task<PictureServiceModel> GetPictureByIdAsync(int pictureId)
        {
            if(pictureId <= 0)
            {
                return null;
            }

            var picture = await this.db.Pictures
                .SingleOrDefaultAsync(p => p.Id == pictureId);

            if (picture == null)
            {
                return null;
            }

            var pictureToReturn = new PictureServiceModel
            {
                Id = picture.Id,
                PictureUrl = picture.PictureUrl,
                ProductId = picture.ProductId
            };

            return pictureToReturn;
        }

        public async Task<IEnumerable<PictureServiceModel>> GetPicturesOfProductByProductId(int productId)
        {
            if (productId <= 0)
            {
                return null;
            }

            var pictures = await this.db.Pictures
                .Where(p => p.ProductId == productId
                && p.IsDeleted == false)
                .Select(p => new PictureServiceModel
                {
                    Id = p.Id,
                    PictureUrl = p.PictureUrl,
                    ProductId = productId
                }).ToListAsync();

            return pictures;
        }

        public async Task<PictureServiceModel> GetPictureByUrl(string pictureUrl)
        {
            if(pictureUrl == null)
            {
                return null;
            }

            var picture = await this.db.Pictures
                .SingleOrDefaultAsync(p => p.PictureUrl == pictureUrl);

            if(picture == null)
            {
                return null;
            }

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
            if(pictureId <= 0 || productId <= 0)
            {
                return false;
            }

            var picture = this.db.Pictures
                .SingleOrDefault(p => p.Id == pictureId);

            if(picture == null)
            {
                return false;
            }

            var product = this.db.Products
                .SingleOrDefault(p => p.Id == productId);

            if(product == null)
            {
                return false;
            }

            this.db.Pictures.Update(picture);
            int result = await this.db.SaveChangesAsync();

            bool pictureIsUpdated = result > 0;

            return pictureIsUpdated;
        }

        public async Task<bool> DeletePicturesOfDeletedProductAsync(int productId)
        {
            if (productId <= 0)
            {
                return false;
            }

            var picturesToDelete = await this.db.Pictures
                .Where(p => p.ProductId == productId)
                .ToListAsync();

            if(picturesToDelete.Count() < 1)
            {
                return true;
            }

            foreach (var picture in picturesToDelete)
            {
                picture.IsDeleted = true;
            }

            this.db.UpdateRange(picturesToDelete);
            int result = await this.db.SaveChangesAsync();

            bool picturesAreDeleted = result > 0;

            return picturesAreDeleted;
        }
    }
}
