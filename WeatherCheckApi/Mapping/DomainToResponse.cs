using AutoMapper;
using WeatherCheck.Domain.Entities;
using WeatherCheckApi.Models.Entities;

namespace WeatherCheckApi.Mapping
{
    public class DomainToResponse : Profile
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public DomainToResponse()
        {
            CreateMap<Weather, WeatherHistoryDto>();
        }
    }
}
