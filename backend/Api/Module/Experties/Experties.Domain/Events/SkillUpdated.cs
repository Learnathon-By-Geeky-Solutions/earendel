using TalentMesh.Framework.Core.Domain.Events;

namespace TalentMesh.Module.Experties.Domain.Events;
public sealed record SkillUpdated : DomainEvent
{
    public Skill? Skill { get; set; }
}
