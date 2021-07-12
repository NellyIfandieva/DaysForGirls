namespace DaysForGirls.Services
{
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPictureService
    {
        Task<PictureServiceModel> GetPictureByIdAsync(int id);

        Task<IEnumerable<PictureServiceModel>> GetPicturesOfProductByProductId(int productId);

        Task<bool> DeletePicturesOfDeletedProductAsync(int productId);
    }
}
