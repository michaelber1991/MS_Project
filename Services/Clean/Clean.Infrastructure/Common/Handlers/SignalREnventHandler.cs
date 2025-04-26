using Clean.Application.Interfaces;
using Clean.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Clean.Infrastructure.Common.Handlers;

public class SignalREventHandler<T>(IHubContext<NotificationHub> hubContext) : INotificationHandler<T>
    where T : class, ISignalREvent, INotification
{
    public async Task Handle(T notification, CancellationToken cancellationToken)
    {
        await hubContext.Clients.All.SendAsync(notification.EventName, notification.Payload, cancellationToken);
    }
}