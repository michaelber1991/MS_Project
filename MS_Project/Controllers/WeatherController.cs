using Microsoft.AspNetCore.Mvc;
using MS_Project.Dtos;
using MS_Project.Services.Interfaces;

namespace MS_Project.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;
    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }       
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<GetWeatherReponse>> Get()
    {
        try
        {
            var forecast = _weatherService.GetWeather();

            return Ok(forecast);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred: " + ex.Message);
        }
    }
}