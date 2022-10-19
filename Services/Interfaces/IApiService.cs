using partner_aluro.Models;

namespace partner_aluro.Services.Interfaces
{
    public interface IApiService
    {
        WeatherResponse Get(string city);
    }
}
