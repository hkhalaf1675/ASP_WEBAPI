namespace Payment_Paypal_Integration.Models.OrderResponse
{
    public class OrderResponseData
    {
        public string id { get; set; }
        public string intent { get; set; }
        public string status { get; set; }
        public List<PurchaseUnit> purchase_units { get; set; }
        public DateTime create_time { get; set; }
        public List<Link> links { get; set; }
    }
}
