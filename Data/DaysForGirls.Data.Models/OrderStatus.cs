namespace DaysForGirls.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class OrderStatus : BaseModel<int>
    {
        [Required]
        public string Name { get; set; }
    }
}
