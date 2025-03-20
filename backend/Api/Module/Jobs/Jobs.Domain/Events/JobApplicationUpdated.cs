
using TalentMesh.Framework.Core.Domain.Events;
using System.Diagnostics.CodeAnalysis;


namespace TalentMesh.Module.Job.Domain.Events
{
    [ExcludeFromCodeCoverage]
    public sealed record JobApplicationUpdated : DomainEvent
    {
        public JobApplication? JobApplication { get; set; }
    }

}
