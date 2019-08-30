using System.ComponentModel.DataAnnotations;

namespace DaysForGirls.Web.ViewModels
{
    public class ManufacturerDisplayAllViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Logo { get; set; }

        [Display(Name = "Is Deleted")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Products")]
        public int ProductsCount { get; set; }


    }
}
