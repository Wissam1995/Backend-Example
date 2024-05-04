using Microsoft.Extensions.Configuration;
using System.Security.Principal;
using WeatherCheck.Data.Interfaces;
using WeatherCheck.Data.Maps;
using WeatherCheck.Data.Repositories;
using WeatherCheck.Domain;

namespace WeatherCheck.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IConfiguration _configuration;

        public UnitOfWork(IConfiguration configuration)
        {
            _configuration = configuration;

            var conf = _configuration.GetConnectionString(Constants.DbConnectionName);
             Weather = new WeatherRepository(conf);

        }

        public IWeatherRepository Weather { get; }
    }
}