using TalentMesh.Framework.Core.Domain.Events;

namespace TalentMesh.Module.Interviews.Domain.Events;
public sealed record InterviewFeedbackCreated : DomainEvent
{
    public InterviewFeedback? InterviewFeedback { get; set; }
}
