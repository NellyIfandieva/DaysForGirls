using DaysForGirls.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DaysForGirls.Services
{
    public interface IPictureService
    {
        Task<bool> Create(List<PictureServiceModel> pictureServiceModels, int ProductId);
        Task<bool> UpdatePictureInfoAsync(int pictureId, int productId);

        Task<PictureServiceModel> GetPictureByIdAsync(int id);
    }
}
