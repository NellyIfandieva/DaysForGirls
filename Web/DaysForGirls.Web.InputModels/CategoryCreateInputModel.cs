using System.ComponentModel.DataAnnotations;

namespace DaysForGirls.Web.InputModels
{
    public class CategoryCreateInputModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
