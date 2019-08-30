using System.ComponentModel.DataAnnotations;

namespace DaysForGirls.Web.InputModels
{
    public class CustomerReviewInputModel
    {
        private const int MinProductIdValue = 1;
        private const int MaxProductIdValue = int.MaxValue;
        private const string RequiredErrorMessage = "The field is required";

        [Required(ErrorMessage = RequiredErrorMessage)]
        public string Title { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        public string Text { get; set; }

        [Range(MinProductIdValue, MaxProductIdValue)]
        public int ProductId { get; set; }
    }
}
