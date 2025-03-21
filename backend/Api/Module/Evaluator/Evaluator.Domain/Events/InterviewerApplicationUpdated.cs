using TalentMesh.Framework.Core.Domain.Events;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Evaluator.Domain.Events
{
    [ExcludeFromCodeCoverage]
    public sealed record InterviewerApplicationUpdated : DomainEvent
    {
        public InterviewerApplication? InterviewerApplication { get; set; }
    }
}
