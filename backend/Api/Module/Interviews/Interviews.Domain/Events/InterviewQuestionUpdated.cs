using TalentMesh.Framework.Core.Domain.Events;

namespace TalentMesh.Module.Interviews.Domain.Events;
public sealed record InterviewQuestionUpdated : DomainEvent
{
    public InterviewQuestion? InterviewQuestion { get; set; }
}
