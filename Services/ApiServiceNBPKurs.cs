using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using System.Net;
using Newtonsoft.Json;

namespace partner_aluro.Services
{
    public class ApiServiceNBPKurs : IApiServiceNBPKurs
    {
        public decimal Get(string code)
        {
            //string url = string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}", city, API_KEY);

            //string a = "a"; //Tabela A kursów średnich walut obcych, 

            //string url = string.Format("http://api.nbp.pl/api/exchangerates/tables/@a/today/"); // blad 404 bo jeszcze nie ma publikacji waluty na dzisiaj.

            string url = $"http://api.nbp.pl/api/exchangerates/rates/a/"+@code+"/last/1"; // blad 404 bo jeszcze nie ma publikacji waluty na dzisiaj.

            //string url = string.Format("http://api.nbp.pl/api/exchangerates/rates/a/pl/last/2/");

            //var web = new WebClient();
            WebClient web = new WebClient();
            var response = web.DownloadString(url);

            //string jsonString = JsonSerializer.Serialize(response);


            var myDeserializedClass = JsonConvert.DeserializeObject<Kurs>(response); // json2csharp
            //var myDeserializedClass = JsonConvert.DeserializeObject<List<KursResponse>>(response); // json2csharp
            Core.Constants.Euro = myDeserializedClass;

            var value = Core.Constants.Euro.rates[0].mid;
            //myDeserializedClass.main.temp_min = myDeserializedClass.main.temp_min - 273.15;
            //myDeserializedClass.main.temp_max = myDeserializedClass.main.temp_max - 273.15;
            Core.Constants.Eur = (decimal)value;
            return (decimal)value;
            //return myDeserializedClass;
        }

    }
}
