using TalentMesh.Framework.Core.Domain.Events;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Notifications.Domain.Events;
[ExcludeFromCodeCoverage]
public sealed record NotificationUpdated : DomainEvent
{
    public Notification? Notification { get; set; }
}
