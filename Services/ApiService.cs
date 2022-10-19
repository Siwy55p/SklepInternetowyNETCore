using Newtonsoft.Json;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using System.Net;

namespace partner_aluro.Services
{
    public class ApiService : IApiService
    {
        private const string API_KEY = "2e39b1498a429d7d336254553cd0c661";
        public WeatherResponse Get(string city)
        {
            string url = string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}", city, API_KEY);

            //var web = new WebClient();
            var web = new WebClient();
            var response = web.DownloadString(url);

            var myDeserializedClass = JsonConvert.DeserializeObject<WeatherResponse>(response); // json2csharp

            myDeserializedClass.main.temp_min = myDeserializedClass.main.temp_min - 273.15;
            myDeserializedClass.main.temp_max = myDeserializedClass.main.temp_max - 273.15;


            return myDeserializedClass;
        }

    }
}
