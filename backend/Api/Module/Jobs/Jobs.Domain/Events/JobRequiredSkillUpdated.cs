using TalentMesh.Framework.Core.Domain.Events;

namespace TalentMesh.Module.Job.Domain.Events;
public sealed record JobRequiredSkillUpdated : DomainEvent
{
    public JobRequiredSkill? JobRequiredSkill { get; set; }
}
