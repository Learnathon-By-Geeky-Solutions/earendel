using TalentMesh.Framework.Core.Domain.Events;

namespace TalentMesh.Module.Notifications.Domain.Events;
public sealed record NotificationUpdated : DomainEvent
{
    public Notification? Notification { get; set; }
}
