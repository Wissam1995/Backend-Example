namespace WeatherCheck.Domain.Entities
{
    public class Weather
    {
        public Weather()
        {

        }
        public Weather(double temp_c, double temp_f, double wind_mph, double wind_kph, double humidity, double cloud, string name, string region, string country, DateTime localTime)
        {
            Id = Guid.NewGuid();
            Temp_c = temp_c;
            Temp_f = temp_f;
            Wind_mph = wind_mph;
            Wind_kph = wind_kph;
            Humidity = humidity;
            Cloud = cloud;
            Name = name;
            Region = region;
            Country = country;
            LocalTime = localTime;
            DateCreated = DateTime.Now;
        }

        public Guid Id { get; set; }
        public double Temp_c { get; set; }
        public double Temp_f { get; set; }
        public double Wind_mph { get; set; }
        public double Wind_kph { get; set; }
        public double Humidity { get; set; }
        public double Cloud { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public DateTime LocalTime { get; set; }
        public DateTime DateCreated { get; set; }
    }
}