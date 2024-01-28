namespace GlobalWeatherApi.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class WeatherInfo
    {
        [JsonProperty("weather")]
        public List<Weather> WeatherConditions { get; set; }

        [JsonProperty("main")]
        public Main TemperatureDetails { get; set; }

        public string TemperatureClassification { get; private set; }

        public void ClassifyTemperature()
        {
            double tempKelvin = TemperatureDetails.Temp;
            if (tempKelvin < 283)
            {
                TemperatureClassification = "Cold";
            }
            else if (tempKelvin >= 283 && tempKelvin <= 298)
            {
                TemperatureClassification = "Warm";
            }
            else
            {
                TemperatureClassification = "Hot";
            }
        }
    }

    public class Weather
    {
        [JsonProperty("main")]
        public string Main { get; set; } // Main weather condition (Clouds, Rain, etc.)

        [JsonProperty("description")]
        public string Description { get; set; } // Description of the weather condition
    }

    public class Main
    {
        [JsonProperty("temp")]
        public double Temp { get; set; } // Temperature in Kelvin
    }
}
