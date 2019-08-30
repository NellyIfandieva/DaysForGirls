using System.ComponentModel.DataAnnotations;

namespace DaysForGirls.Web.InputModels
{
    public class ProductTypeCreateInputModel
    {
        private const string RequiredFieldErrorMessage = "The field is required";

        [Required(ErrorMessage = RequiredFieldErrorMessage)]
        public string Name { get; set; }
    }
}
