using System.ComponentModel.DataAnnotations;

namespace DaysForGirls.Services.Models
{
    public class OrderStatusServiceModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
