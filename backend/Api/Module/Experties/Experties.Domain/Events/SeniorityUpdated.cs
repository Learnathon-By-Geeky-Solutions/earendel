using TalentMesh.Framework.Core.Domain.Events;

namespace TalentMesh.Module.Experties.Domain.Events;
public sealed record SeniorityUpdated : DomainEvent
{
    public Seniority? Seniority { get; set; }
}
