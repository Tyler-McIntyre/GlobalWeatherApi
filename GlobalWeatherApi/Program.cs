using GlobalWeatherApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Load Weather API Settings
IConfigurationSection configSection = builder.Configuration.GetSection("WeatherApiSettings");
WeatherApiSettings weatherApiSettings = configSection.Get<WeatherApiSettings>();
builder.Services.Configure<WeatherApiSettings>(configSection);

builder.Services.AddHttpClient("WeatherClient", client =>
{
    client.BaseAddress = new Uri(weatherApiSettings.BaseUrl);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
