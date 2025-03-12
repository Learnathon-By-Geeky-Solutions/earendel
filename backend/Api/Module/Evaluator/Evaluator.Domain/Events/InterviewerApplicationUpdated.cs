using TalentMesh.Framework.Core.Domain.Events;

namespace TalentMesh.Module.Evaluator.Domain.Events
{
    public sealed record InterviewerApplicationUpdated : DomainEvent
    {
        public InterviewerApplication? InterviewerApplication { get; set; }
    }
}
