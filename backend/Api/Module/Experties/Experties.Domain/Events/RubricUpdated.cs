using TalentMesh.Framework.Core.Domain.Events;

namespace TalentMesh.Module.Experties.Domain.Events;
public sealed record RubricUpdated : DomainEvent
{
    public Rubric? Rubric { get; set; }
}
