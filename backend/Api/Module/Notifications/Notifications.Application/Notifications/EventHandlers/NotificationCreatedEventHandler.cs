using TalentMesh.Module.Notifications.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Notifications.Application.Notifications.EventHandlers;

[ExcludeFromCodeCoverage]
public class NotificationCreatedEventHandler(ILogger<NotificationCreatedEventHandler> logger) : INotificationHandler<NotificationCreated>
{
    public async Task Handle(NotificationCreated notification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("handling Notification created domain event..");
        await Task.FromResult(notification);
        logger.LogInformation("finished handling Notification created domain event..");
    }
}
