using TalentMesh.Framework.Core.Domain.Events;

namespace TalentMesh.Module.Job.Domain.Events;
public sealed record JobUpdated : DomainEvent
{
    public Jobs? User { get; set; }
}
