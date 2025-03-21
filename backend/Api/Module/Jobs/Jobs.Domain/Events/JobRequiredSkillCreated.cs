using TalentMesh.Framework.Core.Domain.Events;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Job.Domain.Events;
[ExcludeFromCodeCoverage]
public sealed record JobRequiredSkillCreated : DomainEvent
{
    public JobRequiredSkill? JobRequiredSkill { get; set; }
}
