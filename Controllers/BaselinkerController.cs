using BaseLinkerApi;
using BaseLinkerApi.Requests.ProductsStorage;
using ClosedXML;
using com.sun.org.glassfish.gmbal;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using partner_aluro.Data;
using partner_aluro.Models;
using System.Net;
using System.Text;
using Requests = BaseLinkerApi.Requests;


namespace partner_aluro.Controllers
{
    public class BaselinkerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;
        public BaselinkerController(ApplicationDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient("BaseLinkerApi");
        }

        const string token = "4004177-4013684-EZM5YKUQATWCYDFCV8HOWL6PHLIAN40Z9YR6ZVVVLYXWY5M84CRULWXPBBUXD4L0";

        private const string BaseUrl = "https://api.baselinker.com/connector.php";

        public async Task<IActionResult> Index()
        {
            using var httpClient = new HttpClient();
            var baseLinkerClient = new BaseLinkerApiClient(httpClient, token);

            var response = await baseLinkerClient.SendAsync(new Requests.CourierShipments.GetCouriersList());

            var list = baseLinkerClient.SendAsync(new Requests.ProductsStorage.GetStoragesList());

            //id =500
           var send = new AddProduct()
            {
                Sku = "123",
                Name = "TestApi",
            };

            var addproduct = baseLinkerClient.SendAsync(send);


            // Return products as JSON
            return Ok();
        }

        public async Task<IActionResult> AddProductToBaselinker()
        {
            string url = "https://api.baselinker.com/connector.php";
            string token = "4004177-4013684-EZM5YKUQATWCYDFCV8HOWL6PHLIAN40Z9YR6ZVVVLYXWY5M84CRULWXPBBUXD4L0";
            string action = "addInventoryProducts";
            string storageId = "shop_31489";


            List<Product> products = _context.Products.ToList();

            string productData = JsonConvert.SerializeObject(products);

            // Build the request data
            string requestData = $"token={token}&method={action}&storage_id={storageId}&products={WebUtility.UrlEncode(productData)}";

            // Send the request
            using (WebClient client = new WebClient())
            {
                //client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                //string response = Encoding.UTF8.GetString(client.UploadString(url, requestData));
                //Console.WriteLine(response);

                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                byte[] responseBytes = Encoding.UTF8.GetBytes(client.UploadString(url, requestData));
                string response = Encoding.UTF8.GetString(responseBytes);
                Console.WriteLine(response);

            }

            return Ok();

        }

        public class ProductDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public int Stock { get; set; }
            public string Ean { get; set; }
            public string Sku { get; set; }
            public string Description { get; set; }
        }




}

}
