using Auth.Domain.Entities;

namespace Auth.Application.Interfaces.Services;

public interface IJwtGeneratorService
{
    string? GenerateToken(User user, List<string> roles);
}