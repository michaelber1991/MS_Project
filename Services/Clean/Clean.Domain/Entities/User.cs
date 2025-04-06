using Clean.Domain.Common;

namespace Clean.Domain.Entities;

public class User : BaseEntity
{
    public required string Name { get; init; }
    public required string Email { get; init; }
}