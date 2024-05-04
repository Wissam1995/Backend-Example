using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherCheck.Domain.Entities;

namespace WeatherCheck.Data.Interfaces
{
    public interface IWeatherRepository
    {
        Task<List<Weather>> GetWeatherHistoryAsync(string cityName);
       Task SaveWeatherDataAsync(Weather weatherDataList);
    }
}
