using DaysForGirls.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaysForGirls.Services
{
    public interface IPictureService
    {
        Task<bool> Create(List<PictureServiceModel> pictureServiceModels, int ProductId);

        Task<bool> UpdatePictureInfoAsync(int pictureId, int productId);

        Task<PictureServiceModel> GetPictureByIdAsync(int id);

        IQueryable<PictureServiceModel> GetPicturesOfProductByProductId(int productId);

        Task<PictureServiceModel> GetPictureByUrl(string pictureUrl);

        Task<bool> DeletePicturesOfDeletedProductAsync(int productId);

        //Task<bool> DeletePictureWithUrl(string pictureUrl);
    }
}
