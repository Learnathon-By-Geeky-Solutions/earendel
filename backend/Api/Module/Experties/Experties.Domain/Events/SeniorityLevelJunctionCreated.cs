using TalentMesh.Framework.Core.Domain.Events;

namespace TalentMesh.Module.Experties.Domain.Events;
public sealed record SeniorityLevelJunctionCreated : DomainEvent
{
    public SeniorityLevelJunction? SeniorityLevelJunction { get; set; }
}