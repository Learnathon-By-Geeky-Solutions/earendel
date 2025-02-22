using TalentMesh.Framework.Core.Domain.Events;

namespace TalentMesh.Module.Experties.Domain.Events;
public sealed record SubSkillUpdated : DomainEvent
{
    public SubSkill? SubSkill { get; set; }
}
