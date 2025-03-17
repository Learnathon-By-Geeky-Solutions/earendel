using TalentMesh.Framework.Core.Domain.Events;

namespace TalentMesh.Module.Job.Domain.Events;
public sealed record JobRequiredSubskillUpdated : DomainEvent
{
    public JobRequiredSubskill? JobRequiredSubskill { get; set; }
}
