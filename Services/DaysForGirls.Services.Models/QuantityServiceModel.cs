namespace DaysForGirls.Services.Models
{
    public class QuantityServiceModel
    {
        public int Id { get; set; }

        public int AvailableItems { get; set; }

        public ProductServiceModel Product { get; set; }
    }
}
