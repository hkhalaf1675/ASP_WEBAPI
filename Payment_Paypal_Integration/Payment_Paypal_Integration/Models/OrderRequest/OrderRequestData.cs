using Payment_Paypal_Integration.Models.OrderResponse;

namespace Payment_Paypal_Integration.Models.OrderRequest
{
    public class OrderRequestData
    {
        public string? intent { get; set; }
        public List<PurchaseUnit?> purchase_units { get; set; }
        public ApplicationContext? application_context { get; set; }
    }
}
