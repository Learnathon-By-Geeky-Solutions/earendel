using TalentMesh.Framework.Core.Domain.Events;

namespace TalentMesh.Module.Evaluator.Domain.Events
{
    public sealed record InterviewerAvailabilityUpdated : DomainEvent
    {
        public InterviewerAvailability? InterviewerAvailability { get; set; }
    }
}
