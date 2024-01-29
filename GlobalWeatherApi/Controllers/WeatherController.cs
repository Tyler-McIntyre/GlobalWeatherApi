using GlobalWeatherApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace GlobalWeatherApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly ILogger<WeatherController> _logger;
        private readonly WeatherApiSettings _weatherApiSettings;

        public WeatherController(WeatherApiSettings weatherApiSettings, ILogger<WeatherController> logger)
        {
            _logger = logger;
            _weatherApiSettings = weatherApiSettings;
        }

        /// <summary>
        /// This endpoint fetches and returns current weather data for a given location, 
        /// defaulting to Raleigh. It includes temperature details, weather conditions, 
        /// and a temperature classification.
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns>General weather info</returns>
        /// <exception cref="Exception"></exception>
        [HttpGet(Name = "GetWeatherInfo")]
        public async Task<ActionResult<WeatherInfo>> Get(float latitude = 35.7796f, float longitude = 78.6382f)
        {
            _logger.LogInformation("Received weather request for Latitude: {Latitude}, Longitude: {Longitude}", latitude, longitude);

            // Build request
            string queryString = $"?lat={latitude}&lon={longitude}&appid={_weatherApiSettings.ApiKey}";
            string url = _weatherApiSettings.BaseUrl + _weatherApiSettings.Endpoint + queryString;

            try
            {
                // Make request
                HttpClient httpClient = new();
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseContent = await response.Content.ReadAsStringAsync();
                WeatherInfo? weatherInfo = JsonConvert.DeserializeObject<WeatherInfo>(responseContent);

                // Deserialization failed
                if (weatherInfo is null) return StatusCode(500);

                weatherInfo.ClassifyTemperature();
                return weatherInfo;
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal Server Error");
                throw;
            }
        }
    }
}
