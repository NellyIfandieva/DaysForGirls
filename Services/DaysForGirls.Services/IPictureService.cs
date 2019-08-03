using DaysForGirls.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DaysForGirls.Services
{
    public interface IPictureService
    {
        Task<int> Create(PictureServiceModel model);
        Task<bool> UpdatePictureInfoAsync(int pictureId, int productId);
    }
}
