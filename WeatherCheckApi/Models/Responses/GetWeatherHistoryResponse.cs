using WeatherCheckApi.Models.Entities;

namespace WeatherCheckApi.Models.Responses
{
    public class GetCurrentWeatherApiResponse: BasicResponseApi
    {
        public WeatherDto Result { get; set; }

    }
}
