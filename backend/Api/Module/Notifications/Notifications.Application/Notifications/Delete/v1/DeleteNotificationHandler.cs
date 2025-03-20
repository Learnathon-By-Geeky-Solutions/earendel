using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Notifications.Domain;
using TalentMesh.Module.Notifications.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Notifications.Application.Notifications.Delete.v1;

public sealed class DeleteNotificationHandler(
    ILogger<DeleteNotificationHandler> logger,
    [FromKeyedServices("notifications:notification")] IRepository<Notification> repository)
    : IRequestHandler<DeleteNotificationCommand>
{
    public async Task Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var notification = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (notification == null)
            throw new NotificationNotFoundException(request.Id);

        await repository.DeleteAsync(notification, cancellationToken);
        logger.LogInformation("Notification with id: {NotificationId} deleted", notification.Id);
    }
}
