using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payment_Paypal_Integration.Models.OrderRequest;
using Payment_Paypal_Integration.Models.OrderResponse;
using Payment_Paypal_Integration.Services;

namespace Payment_Paypal_Integration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        PayPalClientApi client = new PayPalClientApi();
        public PaymentController()
        {

        }

        [HttpGet]
        public async Task<IActionResult> GetAuth()
        {
            var response = await client.GetAuthorizationRequest();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> PostOrder(OrderRequestData orderRequest)
        {
            var authResponse = await client.GetAuthorizationRequest();
            client.SetToken(authResponse.access_token);

            var orderResponse = await client.CreateOrder(orderRequest);

            return Ok(orderResponse);
        }

        
    }
}
