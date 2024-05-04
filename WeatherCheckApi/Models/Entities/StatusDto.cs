using System.ComponentModel;

namespace WeatherCheckApi.Models.Entities
{
    /// <summary>
    ///     User Status DTO
    /// </summary>
    [DisplayName("Status")]
    public class StatusDto
    {
        /// <summary>
        ///     Status Code Number that indicate a specific error
        /// </summary>
        /// <example>Success</example>
        /// <example>Not Found Error</example>
        public string StatusCode { get; set; }

        /// <summary>
        ///     Flag to indicate if the request faced an error or not
        /// </summary>
        public bool IsError { get; set; }

        /// <summary>
        ///     Message
        /// </summary>
        public string Message { get; set; }
    }
}
