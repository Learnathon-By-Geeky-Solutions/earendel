using TalentMesh.Framework.Core.Paging;
using TalentMesh.Module.Notifications.Application.Notifications.Get.v1;
using MediatR;

namespace TalentMesh.Module.Notifications.Application.Notifications.Search.v1;

public class SearchNotificationsCommand : PaginationFilter, IRequest<PagedList<NotificationResponse>>
{
    public Guid? UserId { get; set; }
    public string? Entity { get; set; }
    public string? EntityType { get; set; }
    public string? Message { get; set; }
}
