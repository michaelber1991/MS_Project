namespace MS_Project.Dtos;

public record class GetWeatherReponse(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF { get; init; }
}