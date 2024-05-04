namespace WeatherCheckApi.Models.Entities
{
    public class CurrentWeatherDto
    {
        public double Temp_c { get; set; }
        public double Temp_f { get; set; }
        public double Wind_mph { get; set; }
        public double Wind_kph { get; set; }
        public double Humidity { get; set; }
        public double Cloud { get; set; }

    }
}
