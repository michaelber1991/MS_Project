namespace Auth.Domain.Entities;

public class UserApplication
{
    public int UserId { get; set; }
    public User User { get; set; }

    public int ApplicationId { get; set; }
    public Application Application { get; set; }

    public DateTime CreatedAt { get; set; }
}