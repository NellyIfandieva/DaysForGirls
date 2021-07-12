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

        public async Task<int?> Create(List<PictureServiceModel> pictureServiceModels, int productId)
        {
            if(productId <= 0)
            {
                return null;
            }

            var allPicturesToAddToDb = new List<Picture>();

            foreach (var pSm in pictureServiceModels)
            {
                var picture = new Picture
                {
                    PictureUrl = pSm.PictureUrl,
                    ProductId = productId
                };

                allPicturesToAddToDb.Add(picture);
            }


            this.db.Pictures.AddRange(allPicturesToAddToDb);
            return await this.db.SaveChangesAsync();
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

            return new PictureServiceModel
            {
                Id = picture.Id,
                PictureUrl = picture.PictureUrl,
                ProductId = picture.ProductId
            };
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

            return new PictureServiceModel
            {
                Id = picture.Id,
                PictureUrl = picture.PictureUrl,
                ProductId = picture.ProductId
            };
        }

        public async Task<int?> UpdatePictureInfoAsync(int pictureId, int productId)
        {
            if(pictureId <= 0 || productId <= 0)
            {
                return null;
            }

            var picture = this.db.Pictures
                .SingleOrDefault(p => p.Id == pictureId);

            if(picture == null)
            {
                return null;
            }

            var product = this.db.Products
                .SingleOrDefault(p => p.Id == productId);

            if(product == null)
            {
                return null;
            }

            this.db.Pictures.Update(picture);
            return await this.db.SaveChangesAsync();
        }

        public async Task<int?> DeletePicturesOfDeletedProductAsync(int productId)
        {
            if (productId <= 0)
            {
                return null;
            }

            var picturesToDelete = await this.db.Pictures
                .Where(p => p.ProductId == productId)
                .ToListAsync();

            if(picturesToDelete.Count < 1)
            {
                return null;
            }

            foreach (var picture in picturesToDelete)
            {
                picture.IsDeleted = true;
            }

            this.db.UpdateRange(picturesToDelete);
            return await db.SaveChangesAsync();
        }
    }
}
