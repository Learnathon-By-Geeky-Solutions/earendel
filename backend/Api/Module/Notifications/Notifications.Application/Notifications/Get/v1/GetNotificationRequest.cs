using MediatR;

namespace TalentMesh.Module.Notifications.Application.Notifications.Get.v1;
public class GetNotificationRequest : IRequest<NotificationResponse>
{
    public Guid Id { get; set; }
    public GetNotificationRequest(Guid id) => Id = id;
}
