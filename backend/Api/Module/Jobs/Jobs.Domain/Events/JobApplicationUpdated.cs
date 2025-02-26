
using TalentMesh.Framework.Core.Domain.Events;


namespace TalentMesh.Module.Job.Domain.Events
{

    public sealed record JobApplicationUpdated : DomainEvent
    {
        public JobApplication? JobApplication { get; set; }
    }

}
