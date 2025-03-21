using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Framework.Core.Domain.Events;
using System.Diagnostics.CodeAnalysis;
namespace TalentMesh.Framework.Core.Domain;
[ExcludeFromCodeCoverage]
public abstract class BaseEntity<TId> : IEntity<TId>
{
    public TId Id { get; protected init; } = default!;
    [NotMapped]
    public Collection<DomainEvent> DomainEvents { get; } = new Collection<DomainEvent>();
    public void QueueDomainEvent(DomainEvent @event)
    {
        if (!DomainEvents.Contains(@event))
            DomainEvents.Add(@event);
    }
}

[ExcludeFromCodeCoverage]
public abstract class BaseEntity : BaseEntity<Guid>
{
    protected BaseEntity() => Id = Guid.NewGuid();
}
