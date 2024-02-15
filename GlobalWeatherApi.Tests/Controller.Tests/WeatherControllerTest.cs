using GlobalWeatherApi.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using Newtonsoft.Json;
using GlobalWeatherApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GlobalWeatherApi.Tests
{

    [TestFixture]
    public class GlobalWeatherApi_Controller
    {
        private WeatherController _weatherController;
        private WeatherInfo mockWeatherInfo;

        [SetUp]
        public void Setup()
        {
            mockWeatherInfo = new()
            {
                TemperatureDetails = new Main { Temp = 283.0 },
            };
            mockWeatherInfo.ClassifyTemperature();

            Mock<HttpMessageHandler> mockHttpMessageHandler = new();
            mockHttpMessageHandler.Protected()
               .Setup<Task<HttpResponseMessage>>(
                "SendAsync", 
                ItExpr.IsAny<HttpRequestMessage>(), 
                ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(JsonConvert.SerializeObject(mockWeatherInfo))
               });

            HttpClient httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://test.com"), // Ensure this matches the expected base address in your controller logic
            };

            // Mock IHttpClientFactory
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Mock ILogger and IOptions for WeatherApiSettings
            var mockLogger = new Mock<ILogger<WeatherController>>();
            var mockOptions = new Mock<IOptions<WeatherApiSettings>>();
            mockOptions.Setup(o => o.Value).Returns(new WeatherApiSettings
            {
                BaseUrl = "http://test.com",
                Endpoint = "/weather",
                ApiKey = "testapikey"
            });
            _weatherController = new WeatherController(mockOptions.Object, mockLogger.Object, mockHttpClientFactory.Object);
        }

        [Test]
        public async Task Get_WeatherInfo_Returns_WeatherInfo()
        {
            ActionResult<WeatherInfo> result = await _weatherController.Get(new WeatherInfoRequest());

            Assert.Multiple(() =>
            {
                Assert.That(result?.Value, Is.TypeOf<WeatherInfo>());
                Assert.That(
                    JsonConvert.SerializeObject(result?.Value),
                    Is.EqualTo(JsonConvert.SerializeObject(mockWeatherInfo)
                  ));
            });
        }
    }
}
