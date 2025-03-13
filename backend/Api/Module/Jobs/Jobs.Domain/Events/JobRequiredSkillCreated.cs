using TalentMesh.Framework.Core.Domain.Events;

namespace TalentMesh.Module.Job.Domain.Events;
public sealed record JobRequiredSkillCreated : DomainEvent
{
    public JobRequiredSkill? JobRequiredSkill { get; set; }
}
