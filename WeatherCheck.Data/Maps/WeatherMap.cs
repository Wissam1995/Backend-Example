using Dapper.FluentMap.Mapping;
using WeatherCheck.Domain.Entities;

namespace WeatherCheck.Data.Maps
{
    public class WeatherMap : EntityMap<Weather>
    {
        public WeatherMap()
        {
            Map(t => t.Id).ToColumn("id");
            Map(t => t.Temp_c).ToColumn("temp_c");
            Map(t => t.Temp_f).ToColumn("temp_f");
            Map(t => t.Wind_mph).ToColumn("wind_mph");
            Map(t => t.Wind_kph).ToColumn("wind_kph");
            Map(t => t.Humidity).ToColumn("humidity");
            Map(t => t.Cloud).ToColumn("cloud");
            Map(t => t.Name).ToColumn("name");
            Map(t => t.Region).ToColumn("region");
            Map(t => t.Country).ToColumn("country");
            Map(t => t.LocalTime).ToColumn("local_time");
            Map(t => t.DateCreated).ToColumn("date_created");
        }
    }
}