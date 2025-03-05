using TalentMesh.Framework.Core.Exceptions;

namespace TalentMesh.Module.Notifications.Domain.Exceptions;
public sealed class NotificationNotFoundException : NotFoundException
{
    public NotificationNotFoundException(Guid id)
        : base($"Notification with id {id} not found")
    {
    }
}
