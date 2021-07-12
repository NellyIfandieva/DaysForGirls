namespace DaysForGirls.Services
{
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPictureService
    {
        Task<int?> Create(List<PictureServiceModel> pictureServiceModels, int productId);

        Task<PictureServiceModel> GetPictureByIdAsync(int id);

        Task<IEnumerable<PictureServiceModel>> GetPicturesOfProductByProductId(int productId);

        Task<int?> DeletePicturesOfDeletedProductAsync(int productId);

        Task<int?> UpdatePictureInfoAsync(int pictureId, int productId);
    }
}
