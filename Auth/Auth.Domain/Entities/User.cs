using Auth.Domain.Common;

namespace Auth.Domain.Entities;

public class User : BaseEntity
{
    public required string Name { get; init; }
    public required string Email { get; init; }
}