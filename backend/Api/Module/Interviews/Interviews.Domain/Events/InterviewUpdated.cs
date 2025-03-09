using TalentMesh.Framework.Core.Domain.Events;

namespace TalentMesh.Module.Interviews.Domain.Events;
public sealed record InterviewUpdated : DomainEvent
{
    public Interview? Interview { get; set; }
}
