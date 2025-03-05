using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Notifications.Domain;
using TalentMesh.Module.Notifications.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Notifications.Application.Notifications.Update.v1;

public sealed class UpdateNotificationHandler(
    ILogger<UpdateNotificationHandler> logger,
    [FromKeyedServices("notifications:notification")] IRepository<Notification> repository)
    : IRequestHandler<UpdateNotificationCommand, UpdateNotificationResponse>
{
    public async Task<UpdateNotificationResponse> Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
    {
        // Ensure the request is not null
        ArgumentNullException.ThrowIfNull(request);

        // Fetch the existing notification from the repository
        var notification = await repository.GetByIdAsync(request.Id, cancellationToken);

        // Check if the notification exists and is not deleted
        if (notification is null || notification.DeletedBy != Guid.Empty)
        {
            throw new NotificationNotFoundException(request.Id);
        }

        // Update the notification entity with the new fields
        var updatedNotification = notification.Update(request.UserId, request.Entity, request.EntityType, request.Message);

        // Save the updated notification back to the repository
        await repository.UpdateAsync(updatedNotification, cancellationToken);

        // Log the update action
        logger.LogInformation("Notification with id : {NotificationId} updated.", notification.Id);

        // Return a response containing the updated notification's ID
        return new UpdateNotificationResponse(updatedNotification.Id);
    }
}
