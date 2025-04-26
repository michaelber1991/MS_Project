using Clean.Application.Interfaces;
using MediatR;

namespace Clean.Application.Features.Users.Events;

public class UsersCreatedEvent(int totalUsersCreated) : INotification, ISignalREvent
{
    public int TotalUsersCreated { get; } = totalUsersCreated;
    public string EventName => "UsersCreated";
    public object Payload => new { TotalUsersCreated };
}