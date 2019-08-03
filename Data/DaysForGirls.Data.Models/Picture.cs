namespace DaysForGirls.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Picture : BaseModel<int>
    {
        [Required]
        public string PictureUrl { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
