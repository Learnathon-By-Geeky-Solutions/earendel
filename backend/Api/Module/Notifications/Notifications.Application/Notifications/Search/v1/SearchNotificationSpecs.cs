using Ardalis.Specification;
using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Specifications;
using TalentMesh.Module.Notifications.Application.Notifications.Get.v1;
using TalentMesh.Module.Notifications.Domain;

namespace TalentMesh.Module.Notifications.Application.Notifications.Search.v1;

public class SearchNotificationSpecs : EntitiesByPaginationFilterSpec<Notification, NotificationResponse>
{
    public SearchNotificationSpecs(SearchNotificationsCommand command)
        : base(command) =>
        Query
            .OrderBy(c => c.Entity, !command.HasOrderBy());
}
