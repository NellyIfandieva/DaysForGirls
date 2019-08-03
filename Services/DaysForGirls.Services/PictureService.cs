using DaysForGirls.Data;
using DaysForGirls.Data.Models;
using DaysForGirls.Services.Models;
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
        public async Task<int> Create(PictureServiceModel model)
        {
            Picture picture = new Picture
            {
                PictureUrl = model.PictureUrl
            };

            this.db.Pictures.Add(picture);
            int result = await this.db.SaveChangesAsync();

            if(result != 1)
            {
                return 0;
            }
            return picture.Id;
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
