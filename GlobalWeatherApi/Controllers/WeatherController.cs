using GlobalWeatherApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace GlobalWeatherApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly ILogger<WeatherController> _logger;
        private readonly WeatherApiSettings _weatherApiSettings;
        private HttpClient _httpClient;

        public WeatherController(
            IOptions<WeatherApiSettings> weatherApiSettings,
            ILogger<WeatherController> logger, 
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _weatherApiSettings = weatherApiSettings.Value;
            _httpClient = httpClientFactory.CreateClient("WeatherClient");
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
        public async Task<ActionResult<WeatherInfo>> Get(WeatherInfoRequest req)
        {
            _logger.LogInformation("Received weather request for Latitude: {Latitude}, Longitude: {Longitude}", req.latitude, req.longitude);

            // Build request
            string queryString = $"?lat={req.latitude}&lon={req.longitude}&appid={_weatherApiSettings.ApiKey}";
            string url = _weatherApiSettings.Endpoint + queryString;

            try
            {
                // Make request
                HttpResponseMessage response = await _httpClient.GetAsync(url);
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
                return StatusCode(500);
            }
        }
    }
}
