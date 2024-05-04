using System.ComponentModel.DataAnnotations;

namespace WeatherCheckApi.Models.Entities
{
    public class WeatherRequest
    {
        [Required(ErrorMessage = "You should provide a city name value.")]
        public double Temp_c { get; set; }
        public double Temp_f { get; set; }
        public double Wind_mph { get; set; }
        public double Wind_kph { get; set; }
        public double Humidity { get; set; }
        public double Cloud { get; set; }
        [Required(ErrorMessage = "You should provide a city name value.")]
        public string Name { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public DateTime LocalTime { get; set; }
    }
}
