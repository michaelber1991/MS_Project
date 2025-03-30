using MS_Project.Controllers;
using MS_Project.Dtos;
using MS_Project.Services.Interfaces;

namespace MS_Project.Services;
public class WeatherService: IWeatherService
{
    public IEnumerable<GetWeatherReponse> GetWeather()
    {
        return Enumerable.Range(1, 5).Select(index =>
                new GetWeatherReponse
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    Summaries[Random.Shared.Next(Summaries.Length)]
                ))
            .ToArray();
    }
    
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    
}