using TalentMesh.Framework.Core.Domain.Events;

namespace TalentMesh.Module.Job.Domain.Events;
public sealed record JobCreated : DomainEvent
{
    public Jobs? User { get; set; }
}
