using TalentMesh.Framework.Core.Domain.Events;

namespace TalentMesh.Module.User.Domain.Events;
public sealed record UserUpdated : DomainEvent
{
    public User? User { get; set; }
}
