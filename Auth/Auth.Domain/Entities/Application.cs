using Auth.Domain.Common;

namespace Auth.Domain.Entities;

public class Application : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }

    public ICollection<UserApplication> UserApplications { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }
}