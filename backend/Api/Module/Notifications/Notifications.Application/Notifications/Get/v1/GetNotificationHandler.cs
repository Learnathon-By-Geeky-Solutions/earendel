using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TalentMesh.Module.Notifications.Domain.Exceptions;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using TalentMesh.Module.Notifications.Domain;
using MediatR;

namespace TalentMesh.Module.Notifications.Application.Notifications.Get.v1;

public sealed class GetNotificationHandler(
    [FromKeyedServices("notifications:notificationReadOnly")] IReadRepository<Notification> repository,
    ICacheService cache
) : IRequestHandler<GetNotificationRequest, NotificationResponse>
{
    public async Task<NotificationResponse> Handle(GetNotificationRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var response = await cache.GetOrSetAsync(
            $"notification:{request.Id}",
            async () =>
            {
                var notificationItem = await repository.GetByIdAsync(request.Id, cancellationToken);
                if (notificationItem == null)
                {
                    throw new NotificationNotFoundException(request.Id);
                }

                return new NotificationResponse(
                    notificationItem.Id,
                    notificationItem.UserId,
                    notificationItem.Entity,
                    notificationItem.EntityType,
                    notificationItem.Message
                );
            },
            cancellationToken: cancellationToken
        );
        return response!;
    }
}
