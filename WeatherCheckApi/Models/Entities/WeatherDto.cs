namespace WeatherCheckApi.Models.Entities
{
    public class WeatherDto
    {
        public CurrentWeatherDto current { get; set; }
        public LocationDto location { get; set; }
    }
}
