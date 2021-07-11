namespace DaysForGirls.Web.ViewModels
{
    public class CustomerReviewAdminDisplayAllViewModel
    {

        public int Id { get; set; }

        public string AuthorUsername { get; set; }

        public string AuthorId { get; set; }

        public string CreatedOn { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public bool IsDeleted { get; set; }

        public int ProductId { get; set; }
    }
}
