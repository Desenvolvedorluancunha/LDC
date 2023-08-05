using LDC.Data;

namespace LDC.Services;
public class WeatherForecastService
{
    private static readonly string[] Summaries = new[]
    {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

    private readonly Random _random;

    public WeatherForecastService()
    {
        _random = new Random();
    }

    public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
    {
        return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = startDate.AddDays(index),
            TemperatureC = _random.Next(-20, 55),
            Summary = Summaries[_random.Next(Summaries.Length)]
        }).ToArray());
    }
}
