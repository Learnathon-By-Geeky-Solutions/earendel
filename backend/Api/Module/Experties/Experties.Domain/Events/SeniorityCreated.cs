using TalentMesh.Framework.Core.Domain.Events;

namespace TalentMesh.Module.Experties.Domain.Events;
public sealed record SeniorityCreated : DomainEvent
{
    public Seniority? Seniority { get; set; }
}
