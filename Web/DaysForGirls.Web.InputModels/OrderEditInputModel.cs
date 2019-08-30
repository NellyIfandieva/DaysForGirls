namespace DaysForGirls.Web.InputModels
{
    public class OrderEditInputModel
    {
        public string Id { get; set; }

        public string DeliveryEarlistDate { get; set; }

        public string DeliveryLatestDate { get; set; }

        public string IssuedOn { get; set; }

        public string UserIssuedTo { get; set; }

        public string OrderStatus { get; set; }

        public decimal TotalPrice { get; set; }

        public int OrderedProductsNum { get; set; }
    }
}
