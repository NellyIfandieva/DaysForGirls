namespace DaysForGirls.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;

    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary cloudnaryUtility;

        public CloudinaryService(Cloudinary cloudnaryUtility)
        {
            this.cloudnaryUtility = cloudnaryUtility;
        }

        public async Task<string> UploadPictureForProductAsync(
            IFormFile image, 
            string fileName)
        {
            byte[] destinationData;

            using (var ms = new MemoryStream())
            {
                await image.CopyToAsync(ms);
                destinationData = ms.ToArray();
            }

            UploadResult uploadResult = null;

            using (var ms = new MemoryStream(destinationData))
            {
                ImageUploadParams uploadParams = new ImageUploadParams
                {
                    Folder = "ImagesOfProducts",
                    File = new FileDescription(fileName, ms)
                };

                uploadResult = this.cloudnaryUtility.Upload(uploadParams);
            }

            return uploadResult?.SecureUri.AbsoluteUri;
        }

        public async Task<string> UploadPictureForSaleAsync(IFormFile image, string fileName)
        {
            byte[] destinationData;

            using (var ms = new MemoryStream())
            {
                await image.CopyToAsync(ms);
                destinationData = ms.ToArray();
            }

            UploadResult uploadResult = null;

            using (var ms = new MemoryStream(destinationData))
            {
                ImageUploadParams uploadParams = new ImageUploadParams
                {
                    Folder = "ImagesForSales",
                    File = new FileDescription(fileName, ms)
                };

                uploadResult = this.cloudnaryUtility.Upload(uploadParams);
            }

            return uploadResult?.SecureUri.AbsoluteUri;
        }
    }
}
