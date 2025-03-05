using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Notifications.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Notifications.Application.Notifications.Create.v1;

public sealed class CreateNotificationHandler(
    ILogger<CreateNotificationHandler> logger,
    [FromKeyedServices("notifications:notification")] IRepository<Notification> repository)
    : IRequestHandler<CreateNotificationCommand, CreateNotificationResponse>
{
    public async Task<CreateNotificationResponse> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var notification = Notification.Create(
            request.UserId,
            request.Entity,
            request.EntityType,
            request.Message
        );

        await repository.AddAsync(notification, cancellationToken);
        logger.LogInformation("Notification created {NotificationId}", notification.Id);

        return new CreateNotificationResponse(notification.Id);
    }
}
