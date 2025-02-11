using TalentMesh.Framework.Core.Domain.Events;

namespace TalentMesh.Module.User.Domain.Events;
public sealed record UserCreated : DomainEvent
{
    public User? User { get; set; }
}
