using AutoMapper;
using Jaeger.Propagation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;
using System.Numerics;
using System.Reflection.Metadata;
using WeatherCheck.Data.Interfaces;
using WeatherCheck.Data.Repositories;
using WeatherCheck.Domain;
using WeatherCheck.Domain.Entities;
using WeatherCheckApi.Models.Entities;
using WeatherCheckApi.Models.Responses;
using WeatherCheckApi.Services.WeatherApi;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WeatherCheckApi.Controllers
{
    [Route(Constants.WeatherApiUrl)]
    [ApiController]
    //[Authorize]
    public class WeatherController : BaseController
    {
        private readonly IConfiguration _config;
        private readonly ILogger<WeatherController> _logger;
        private readonly IWeatherApiService _weatherApiService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        ///     Controller constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="weatherApiService"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="configuration"></param>
        public WeatherController(ILogger<WeatherController> logger, IMapper mapper,
            IUnitOfWork unitOfWork, IWeatherApiService weatherApiService,
            IConfiguration configuration)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _weatherApiService = weatherApiService;
            _config = configuration;
        }

        #region Get current weather

        /// <summary>
        ///     API used to get the current weather of a specific city
        /// </summary>
        /// <returns></returns>
        [HttpGet("/current-weather/{cityName}")]
        [ProducesResponseType(typeof(GetCurrentWeatherApiResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasicResponseApi>> GetCurrentWeather(string cityName)
        {
            try
            {
                GetCurrentWeatherApiResponse response = new();
                var httpResponse = await _weatherApiService.GetCurrentWeather(cityName);
                if (httpResponse.ResponseMessage.StatusCode == HttpStatusCode.OK)
                {
                    var getWeatherApiResponse = httpResponse.GetContent();
                    if (getWeatherApiResponse != null)
                    {
                        response.Result = getWeatherApiResponse;
                        response.Status = httpResponse.ResponseMessage.StatusCode.ToString();
                        return Ok(response);
                    }
                }
                response.Status = httpResponse.ResponseMessage.StatusCode.ToString();
                return NotFound(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        #endregion Get current weather


        [HttpPost("/api/weather/savecurrentweather")]
        [ProducesResponseType(typeof(BasicResponseApi), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> SaveCurrentWeatherFromWeatherApi([FromBody] CityRequest request)
        {
            try
            {
                BasicResponseApi response = new();
                var httpResponse = await _weatherApiService.GetCurrentWeather(request.CityName);
                if (httpResponse.ResponseMessage.StatusCode == HttpStatusCode.OK)
                {
                    var result = httpResponse.GetContent();
                    if (result != null)
                    {
                        // Map the WeatherDto back to the Weather entity

                        var weatherData = new Weather(result.current.Temp_c, result.current.Temp_f, result.current.Wind_mph, result.current.Wind_kph, result.current.Humidity, result.current.Cloud
                            , result.location.Name, result.location.Region, result.location.Country, result.location.LocalTime);
                            
                        // Save the weather data to the database
                        await _unitOfWork.Weather.SaveWeatherDataAsync(weatherData);

                        response.Status = HttpStatusCode.OK.ToString();

                        return Ok(response);
                    }
                }
                response.Status = httpResponse.ResponseMessage.StatusCode.ToString();
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("/api/weather/saveWeatherData")]
        [ProducesResponseType(typeof(BasicResponseApi), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> SaveCurrentWeatherFromBody([FromBody] WeatherRequest request)
        {
            try
            {
                BasicResponseApi response = new();
                if (request != null)
                {
                    // Map the WeatherDto back to the Weather entity
                     var weatherData = new Weather(request.Temp_c, request.Temp_f, request.Wind_mph, request.Wind_kph, request.Humidity, request.Cloud
                    , request.Name, request.Region, request.Country, request.LocalTime);
                    // Save the weather data to the database
                    await _unitOfWork.Weather.SaveWeatherDataAsync(weatherData);
                    response.Status = HttpStatusCode.OK.ToString();
                    return Ok(response);
                }
                response.Status = HttpStatusCode.NotFound.ToString();
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("/api/weather/savedhistory")]
        [ProducesResponseType(typeof(GetWeatherHistoryResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<GetWeatherHistoryResponse>> GetSavedWeatherHistory(string cityName)
        {
            try
            {
                GetWeatherHistoryResponse response = new();

                // Fetch the weather history for the specified city from the database
                var weatherHistory = await _unitOfWork.Weather.GetWeatherHistoryAsync(cityName);
                if (weatherHistory.Any())
                {
                    // Map the Weather entities to WeatherDto objects
                    var weatherDtoList = _mapper.Map<List<WeatherHistoryDto>>(weatherHistory);
                    response.Status = HttpStatusCode.OK.ToString();
                    response.Result = weatherDtoList;
                    return Ok(weatherDtoList);
                }
                response.Status = HttpStatusCode.NotFound.ToString();
                return NotFound(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

    }
}