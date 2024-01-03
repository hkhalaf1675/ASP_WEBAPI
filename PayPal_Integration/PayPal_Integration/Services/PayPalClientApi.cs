using Newtonsoft.Json;
using PayPal_Integration.Helper;
using PayPal_Integration.Models;
using System.Net.Http.Headers;
using System.Text;

namespace PayPal_Integration.Services
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

            var byteArray = Encoding.ASCII.GetBytes($"{PaypalConfigrations.ClientId}:{ConfigHelper.ClientSecret}");
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var keyValueParis = new List<KeyValuePair<string, string>>
                { new KeyValuePair<string, string>("grant_type", "client_credentials") };

            var response = await _client.PostAsync($"{PaypalConfigrations.BaseUrl}/v1/oauth2/token", new FormUrlEncodedContent(keyValueParis));

            var responseAsString = await response.Content.ReadAsStringAsync();

            var authorizationResponse = JsonConvert.DeserializeObject<AuthorizationResponseData>(responseAsString);

            return authorizationResponse;
        }
    }
}
