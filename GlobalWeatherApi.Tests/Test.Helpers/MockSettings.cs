using GlobalWeatherApi.Models;

namespace GlobalWeatherApi.Tests.Test.Helpers
{
    public static class MockSettings
    {
        public static readonly WeatherApiSettings MockWeatherApiSettings = new()
        {
            ApiKey = "ApiKey",
            BaseUrl = "BaseUrl",
            Endpoint = "Endpoint"
        };
    }
}
