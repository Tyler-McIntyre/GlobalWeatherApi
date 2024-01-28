# GlobalWeatherApi

## Introduction
GlobalWeatherApi is a .NET Core Web API that provides weather information based on longitude and latitude coordinates.
Built on .NET 6.0, this API interacts with [Open Weather]("https://openweathermap.org/") to fetch current weather data.

## Prerequisites
Ensure you have .NET 6.0 SDK installed on your machine.

## Configuration
Obtain an API key from [OpenWeatherMap API]("https://openweathermap.org/api").

In the appsettings.json file of the solution, add your API key:
```
  "WeatherApiSettings": {
    "ApiKey": "<Your Api Key>",
    "BaseUrl": "https://api.openweathermap.org/data/2.5",
    "Endpoint": "/weather"
  }
```

## Usage
This project is equipped with OpenAPI (Swagger) for easy testing and interaction with the API through a user-friendly interface.
