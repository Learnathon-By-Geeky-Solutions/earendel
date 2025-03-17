using TalentMesh.Framework.Core.Domain.Events;

namespace TalentMesh.Module.Evaluator.Domain.Events
{
    public sealed record InterviewerEntryFormUpdated : DomainEvent
    {
        public InterviewerEntryForm? InterviewerEntryForm { get; set; }
    }
}
