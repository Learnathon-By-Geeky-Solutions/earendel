using TalentMesh.Framework.Core.Domain.Events;

namespace TalentMesh.Module.Experties.Domain.Events;
public sealed record SeniorityLevelJunctionUpdated : DomainEvent
{
    public SeniorityLevelJunction? SeniorityLevelJunction { get; set; }
}