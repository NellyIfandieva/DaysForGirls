using System.ComponentModel.DataAnnotations;

namespace DaysForGirls.Data.Models
{
    public class Category : BaseModel<int>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public bool IsDeleted { get; set; }
    }
}