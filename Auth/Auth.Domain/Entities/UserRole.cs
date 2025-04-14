namespace Auth.Domain.Entities;

public class UserRole
{
    public int UserId { get; set; }
    public User User { get; set; }

    public int ApplicationId { get; set; }
    public Application Application { get; set; }

    public int RoleId { get; set; }
    public Role Role { get; set; }

    public DateTime CreatedAt { get; set; }
}