using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using partner_aluro.Data;
using partner_aluro.Models;
using System.Text;

//szukanaNazwa PostApi
namespace partner_aluro.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaselinkerApiController : ControllerBase
    {
        //private readonly IHttpClientFactory _clientFactory;

        //public BaselinkerApiConstroller(IHttpClientFactory clientFactory)
        //{
        //    _clientFactory = clientFactory;
        //}

        [HttpGet]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            //var client = _clientFactory.CreateClient();
            var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://api.baselinker.com/");

            var parameters = new
            {
                client_id = "partner_aluro",
                access_token = "4004177-4013684-EZM5YKUQATWCYDFCV8HOWL6PHLIAN40Z9YR6ZVVVLYXWY5M84CRULWXPBBUXD4L0"
            };

            var content = new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("method/getProducts", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<IEnumerable<Product>>>(responseContent);

            return apiResponse.Result;
        }

        [HttpPost("send-product")]
        public async Task<IActionResult> SendProduct([FromBody] Product product)
        {
            var client = new HttpClient();
            client.BaseAddress = new System.Uri("https://api.baselinker.com/");

            // Create request parameters
            var requestParams = new
            {
                client_id = "partner_aluro",
                access_token = "4004177-4013684-EZM5YKUQATWCYDFCV8HOWL6PHLIAN40Z9YR6ZVVVLYXWY5M84CRULWXPBBUXD4L0",
                product = new
                {
                    sku = product.EAN13,
                    name = product.Name,
                    price_brutto = product.CenaProduktuBrutto,
                    quantity = product.Ilosc,
                    category_id = product.CategoryId
                }
            };

            // Serialize request parameters
            var requestContent = new StringContent(JsonConvert.SerializeObject(requestParams), Encoding.UTF8, "application/json");

            // Send request and parse response
            var response = await client.PostAsync("method/addProduct", requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<SendProductResponse>>(responseContent);

            // Check for errors in the API response
            if (!apiResponse.Success)
            {
                return BadRequest(apiResponse.Message);
            }

            // Return success response
            return Ok(apiResponse.Result.ProductId);


            return Ok();
        }

    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
    }

    public class SendProductResponse
    {
        public int ProductId { get; set; }
    }
}
