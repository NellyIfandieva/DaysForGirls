namespace DaysForGirls.Data.Models
{
    public class ProductType : BaseModel<int>
    {
        public string Name { get; set; }

        public bool IsDeleted { get; set; }
    }
}