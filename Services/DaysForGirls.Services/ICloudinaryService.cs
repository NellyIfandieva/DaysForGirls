using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DaysForGirls.Services
{
    public interface ICloudinaryService
    {
        Task<string> UploadPictureForProductAsync(IFormFile image, string imageName);
        Task<string> UploadPictureForSaleAsync(IFormFile image, string fileName);
    }
}
