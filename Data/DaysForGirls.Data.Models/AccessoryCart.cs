namespace DaysForGirls.Data.Models
{
    public class AccessoryCart : BaseModel<string>
    {
        public int AccessoryId { get; set; }
        public Accessory Accessory { get; set; }

        public string ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
    }
}