namespace DaysForGirls.Services.Models
{
    public class PictureServiceModel
    {
        public int Id { get; set; }

        public string PictureUrl { get; set; }

        public int ProductId { get; set; }

        public bool IsDeleted { get; set; }
    }
}
