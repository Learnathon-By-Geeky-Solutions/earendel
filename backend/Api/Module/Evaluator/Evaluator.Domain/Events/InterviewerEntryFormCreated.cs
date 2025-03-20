using TalentMesh.Framework.Core.Domain.Events;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Evaluator.Domain.Events
{
    [ExcludeFromCodeCoverage]

    public sealed record InterviewerEntryFormCreated : DomainEvent
    {
        public InterviewerEntryForm? InterviewerEntryForm { get; set; }
    }
}
