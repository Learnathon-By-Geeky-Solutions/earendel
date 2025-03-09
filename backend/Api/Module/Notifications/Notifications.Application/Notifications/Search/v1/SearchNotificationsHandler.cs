using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Notifications.Application.Notifications.Get.v1;
using TalentMesh.Module.Notifications.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace TalentMesh.Module.Notifications.Application.Notifications.Search.v1;

public sealed class SearchNotificationsHandler(
    [FromKeyedServices("notifications:notificationReadOnly")] IReadRepository<Notification> repository)
    : IRequestHandler<SearchNotificationsCommand, PagedList<NotificationResponse>>
{
    public async Task<PagedList<NotificationResponse>> Handle(SearchNotificationsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchNotificationSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<NotificationResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}
