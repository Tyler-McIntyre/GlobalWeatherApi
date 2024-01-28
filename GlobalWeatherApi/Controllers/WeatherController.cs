using GlobalWeatherApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public async Task<WeatherInfo> Get(float latitude = 35.7796f, float longitude = 78.6382f)
        {
            _logger.LogInformation("Received weather request for Latitude: {Latitude}, Longitude: {Longitude}", latitude, longitude);

            // Build request
            string queryString = $"?lat={latitude}&lon={longitude}&appid={_weatherApiSettings.ApiKey}";
            string url = _weatherApiSettings.BaseUrl + _weatherApiSettings.Endpoint + queryString;

            HttpResponseMessage response;
            try
            {
                // Make http request
                HttpClient httpClient = new();
                response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving weather data for Latitude");
                throw;
            }

            // Read content from response and deserialize to WeatherInfo object
            string responseContent = await response.Content.ReadAsStringAsync() ?? throw new Exception("Failed to retrieve weather info");

            WeatherInfo weatherInfo = JsonConvert.DeserializeObject<WeatherInfo>(responseContent)!;
            weatherInfo.ClassifyTemperature();

            _logger.LogInformation("Weather data retrieved successfully");

            return weatherInfo;
        }
    }
}
