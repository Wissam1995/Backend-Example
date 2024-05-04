using WeatherCheckApi.Models.Entities;

namespace WeatherCheckApi.Models.Responses
{
    public class GetWeatherHistoryResponse : BasicResponseApi
    {
        public List<WeatherHistoryDto> Result { get; set; }

    }
}
