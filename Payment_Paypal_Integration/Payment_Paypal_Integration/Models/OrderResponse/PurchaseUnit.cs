using Microsoft.AspNetCore.Mvc.RazorPages;
using Payment_Paypal_Integration.Models.OrderResponseData.OrderResponse;

namespace Payment_Paypal_Integration.Models.OrderResponse
{
    public class PurchaseUnit
    {
        public string? reference_id { get; set; }
        public Amount? amount { get; set; }
        public Payee? payee { get; set; }
    }
}
