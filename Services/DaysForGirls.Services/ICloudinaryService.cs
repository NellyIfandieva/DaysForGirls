namespace DaysForGirls.Services
{
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public interface ICloudinaryService
    {
        Task<string> UploadPictureForProductAsync(IFormFile image, string imageName);
        Task<string> UploadPictureForSaleAsync(IFormFile image, string fileName);
        Task<string> UploadLogoForManufacturerAsync(IFormFile image, string fileName);
    }
}
