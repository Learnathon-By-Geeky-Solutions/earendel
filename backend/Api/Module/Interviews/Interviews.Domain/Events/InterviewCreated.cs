using TalentMesh.Framework.Core.Domain.Events;

namespace TalentMesh.Module.Interviews.Domain.Events;
public sealed record InterviewCreated : DomainEvent
{
    public Interview? Interview { get; set; }
}
