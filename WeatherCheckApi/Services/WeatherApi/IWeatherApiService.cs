using RestEase;
using WeatherCheck.Domain;
using WeatherCheckApi.Models.Entities;

namespace WeatherCheckApi.Services.WeatherApi
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IWeatherApiService
    {
        [AllowAnyStatusCode]
        [Get("/v1/current.json?key=" + Constants.AppToken + "&q={name}")]
        Task<Response<WeatherDto>> GetCurrentWeather([Path] string name);

        [AllowAnyStatusCode]
        [Get("/v1/history.json?key=" + Constants.AppToken + "&q={name}$dt={date}")]
        Task<Response<List<WeatherDto>>> GetHistoryWeather([Path] string name, [Path] DateTime date);
    }
}