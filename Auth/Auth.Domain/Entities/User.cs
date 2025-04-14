using System.Security.Cryptography;
using System.Text;
using Auth.Domain.Common;

namespace Auth.Domain.Entities;

public class User : BaseEntity
{
    public required string Username { get; set; }

    public required string GlobalName { get; set; }
    public string? PasswordHash { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public DateTime CreatedAt { get; init; }

    public ICollection<UserRole> UserRoles { get; init; } = new List<UserRole>();
    public ICollection<UserApplication> UserApplications { get; init; } = new List<UserApplication>();

    public string PasswordSalt { get; private set; }

    private static string GenerateSalt()
    {
        var saltBytes = new byte[16];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(saltBytes);
        return Convert.ToBase64String(saltBytes);
    }

    private static string HashPassword(string password, string salt)
    {
        using var sha256 = SHA256.Create();
        var combined = Encoding.UTF8.GetBytes(password + salt);
        var hash = sha256.ComputeHash(combined);
        return Convert.ToBase64String(hash);
    }

    public void SetPassword(string password)
    {
        PasswordSalt = GenerateSalt();
        PasswordHash = HashPassword(password, PasswordSalt);
    }

    public bool VerifyPassword(string inputPassword)
    {
        var hashedInput = HashPassword(inputPassword, PasswordSalt);
        return hashedInput == PasswordHash;
    }
}