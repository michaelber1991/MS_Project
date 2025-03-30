using MS_Project.Dtos;

namespace MS_Project.Services.Interfaces;

public interface IWeatherService
{
    IEnumerable<GetWeatherReponse> GetWeather();
}