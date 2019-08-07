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
            var picture = this.db.Pictures
                .SingleOrDefault(p => p.Id == id);

            PictureServiceModel pictureToReturn = new PictureServiceModel
            {
                Id = picture.Id,
                PictureUrl = picture.PictureUrl,
                ProductId = picture.ProductId
            };

            await Task.Delay(0);
            return pictureToReturn;
        }

        public IQueryable<PictureServiceModel> GetPicturesOfProductByProductId(int productId)
        {
            var pictures = this.db.Pictures
                .Where(p => p.ProductId == productId)
                .Select(p => new PictureServiceModel
                {
                    PictureUrl = p.PictureUrl
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
    }
}
