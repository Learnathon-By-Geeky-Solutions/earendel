using TalentMesh.Framework.Core.Domain.Events;
using System.Diagnostics.CodeAnalysis;


namespace TalentMesh.Module.Job.Domain.Events
{
    [ExcludeFromCodeCoverage]
    public sealed record JobApplicationCreated : DomainEvent
    {
        public JobApplication? JobApplication { get; set; }
    }

}
