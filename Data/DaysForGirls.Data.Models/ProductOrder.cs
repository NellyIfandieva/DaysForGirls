namespace DaysForGirls.Data.Models
{
    public class ProductOrder : BaseModel<string>
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string OrderId { get; set; }
        public Order Order { get; set; }
    }
}