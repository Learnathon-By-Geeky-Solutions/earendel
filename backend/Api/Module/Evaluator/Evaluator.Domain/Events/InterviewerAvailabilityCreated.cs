using TalentMesh.Framework.Core.Domain.Events;

namespace TalentMesh.Module.Evaluator.Domain.Events
{
    public sealed record InterviewerAvailabilityCreated : DomainEvent
    {
        public InterviewerAvailability? InterviewerAvailability { get; set; }
    }
}
