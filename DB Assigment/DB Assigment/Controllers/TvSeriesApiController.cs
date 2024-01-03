using DB_Assigment.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices.JavaScript;

namespace DB_Assigment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TvSeriesApiController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetApiData(int startYear,int endYear)
        {
            var data = await GetTvSeries(startYear, endYear);

            
            return Ok(data);
        }

        [HttpGet("Test")]
        public async Task<IActionResult> TestApi()
        {
            HttpClient client = new HttpClient();
            string baseUrl = "https://jsonmock.hackerrank.com/api/tvseries";

            var data = client.GetAsync(baseUrl).Result;

            var stringData = data.Content.ReadAsStringAsync().Result;

            dynamic jsonData = JObject.Parse(stringData);

            return Ok(stringData);
        }

        public static async Task<List<string>> GetTvSeries(int startYear, int endYear)
        {
            List<string> seriesNames = new List<string>();
            string baseUrl = "https://jsonmock.hackerrank.com/api/tvseries";
            HttpClient client = new HttpClient();

            var response = await client.GetAsync(baseUrl);
            var stringResponse = await response.Content.ReadAsStringAsync();

            var objectResponse = JsonConvert.DeserializeObject<ApiResponse>(stringResponse);
            var totalPages = objectResponse?.total_pages;

            for(int i=1; i<= totalPages; i++)
            {
                var result = await client.GetAsync($"{baseUrl}?page={i}");
                var stringResult = await result.Content.ReadAsStringAsync();

                var objectResult = JsonConvert.DeserializeObject<ApiResponse>(stringResult);

                foreach(var item in objectResult?.data)
                {
                    var productionYears = item.runtime_of_series.Trim('(', ')').Split('-');
                    
                    int seriesStartYear = 0;
                    int.TryParse(productionYears[0], out seriesStartYear);

                    int seriesEndYear = -1;
                    if(productionYears.Length > 1)
                        int.TryParse(productionYears[1], out seriesEndYear);

                    if (startYear >= seriesStartYear && endYear <= seriesEndYear)
                    {
                        seriesNames.Add(item.name);
                    }
                }
            }

            seriesNames.Sort();

            return seriesNames;
        }
    }
}
