using IdentityModel.Client;
using Newtonsoft.Json;
using Payment_Paypal_Integration.Helper;
using Payment_Paypal_Integration.Models;
using Payment_Paypal_Integration.Models.OrderRequest;
using Payment_Paypal_Integration.Models.OrderResponse;
using PayPal.Api;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

namespace Payment_Paypal_Integration.Services
{
    public class PayPalClientApi
    {
        private HttpClient _client;
        public PayPalClientApi()
        {
            CreateHttpClient();
        }
        private void CreateHttpClient()
        {
            _client = new HttpClient();
        }
        private void EnsureHttpClientCreated()
        {
            if (_client == null)
            {
                CreateHttpClient();
            }
        }
        public async Task<AuthorizationResponseData> GetAuthorizationRequest()
        {
            EnsureHttpClientCreated();

            var byteArray = Encoding.ASCII.GetBytes($"{PaypalConfigrations.ClientId}:{PaypalConfigrations.ClientSecret}");
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var keyValueParis = new List<KeyValuePair<string, string>>
                { new KeyValuePair<string, string>("grant_type", "client_credentials") };

            var response = await _client.PostAsync($"{PaypalConfigrations.BaseUrl}/v1/oauth2/token", new FormUrlEncodedContent(keyValueParis));

            var responseAsString = await response.Content.ReadAsStringAsync();

            var authorizationResponse = JsonConvert.DeserializeObject<AuthorizationResponseData>(responseAsString);

            return authorizationResponse;
        }

        public async Task<OrderResponseData> CreateOrder(OrderRequestData orderRequest )
        {
            EnsureHttpClientCreated();

            var requestContent = JsonConvert.SerializeObject(orderRequest);

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage
            {
                Content = new StringContent(requestContent, Encoding.UTF8, "application/json")
            };

            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            

            _client.DefaultRequestHeaders.Add("PayPal-Request-Id", new Guid().ToString());

            var response = await _client.PostAsJsonAsync($"{PaypalConfigrations.BaseUrl}/v2/checkout/orders", httpRequestMessage.Content);

            var responseAsString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<OrderResponseData>(responseAsString);

            
            return result;
        }

        public async Task<string> CreatePayment(decimal amount)
        {
            var request = new OrdersCreateRequest();
            request.Headers.Add("prefer", "return=representation");

            request.RequestBody(new OrderRequestData()
            {
                intent = "CAPTURE",
                purchase_units = new List<PurchaseUnit?>()
                {
                    new PurchaseUnit()
                    {
                        amount = new Models.OrderResponseData.OrderResponse.Amount()
                        {
                            currency_code = "USD",
                            value = amount.ToString("0.00")
                        }
                    }
                },
                application_context = new ApplicationContext()
                {
                    return_url = "http://localhost:4200/return-url",
                    cancel_url = "http://localhost:4200/cancel-url"
                }
            });

            var client = GetPayPalClient();
            var response = await client.Execute(request);
            var result = response.Result<Order>();

            var approvalUrl = result.Links.FirstOrDefault(link => link.Rel == "approve").Href;
            return approvalUrl;
        }

        public async Task<bool> CapturePayment(string orderId)
        {
            var request = new OrdersCaptureRequest(orderId);
            request.RequestBody(new OrderActionRequest());

            var client = GetPayPalClient();
            var response = await client.Execute(request);
            var result = response.Result<Order>();

            return result.Status == "COMPLETED";
        }

        //public async Task<string?> CaptureOrder()
        //{
        //    HttpRequestMessage httpRequestMessage = new HttpRequestMessage
        //    {
        //        Method = HttpMethod.Post,
        //        RequestUri = new Uri($"{PaypalConfigrations.BaseUrl}/orders/:order_id/capture"),

        //    };
        //}

        public void SetToken(string token)
        {
            _client.SetBearerToken(token);
        }


    }
}
