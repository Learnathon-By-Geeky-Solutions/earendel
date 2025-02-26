using TalentMesh.Framework.Core.Domain.Events;


namespace TalentMesh.Module.Job.Domain.Events
{

    public sealed record JobApplicationCreated : DomainEvent
    {
        public JobApplication? JobApplication { get; set; }
    }

}
