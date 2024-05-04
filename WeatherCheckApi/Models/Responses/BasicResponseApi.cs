using System.ComponentModel;
using WeatherCheckApi.Models.Entities;

namespace WeatherCheckApi.Models.Responses
{
    /// <summary>
    ///     Basic Response API
    /// </summary>
    [DisplayName("Basic API Response")]
    public class BasicResponseApi
    {
        /// <summary>
        ///     Status Object
        /// </summary>
        public string Status { get; set; }
    }
}
