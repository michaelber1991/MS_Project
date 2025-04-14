using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Auth.Application.Interfaces.Services;
using Auth.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Application.Services;

public class JwtGeneratorService : IJwtGeneratorService
{
    public string GenerateToken(User user, List<string> roles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, (user.Name ?? user.Name) ?? string.Empty)
        };

        claims.Add(new Claim("roles", JsonSerializer.Serialize(roles)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("una-clave-secreta-mas-larga-de-256-bits"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            "tu-emisor",
            "tu-front",
            claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}