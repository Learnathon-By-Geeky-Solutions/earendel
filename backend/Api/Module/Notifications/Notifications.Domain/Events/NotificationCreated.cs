using TalentMesh.Framework.Core.Domain.Events;

namespace TalentMesh.Module.Notifications.Domain.Events;
public sealed record NotificationCreated : DomainEvent
{
    public Notification? Notification { get; set; }
}
