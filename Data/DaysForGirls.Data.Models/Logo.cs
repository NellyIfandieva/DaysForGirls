using System.ComponentModel.DataAnnotations;

namespace DaysForGirls.Data.Models
{
    public class Logo : BaseModel<int>
    {
        [Required]
        public string LogoUrl { get; set; }

        public int ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }

        public bool IsDeleted { get; set; }
    }
}
