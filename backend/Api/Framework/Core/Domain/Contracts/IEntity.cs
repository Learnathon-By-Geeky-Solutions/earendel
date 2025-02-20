using System.Collections.ObjectModel;
using TalentMesh.Framework.Core.Domain.Events;

namespace TalentMesh.Framework.Core.Domain.Contracts;

public interface IEntity
{
    Collection<DomainEvent> DomainEvents { get; }
}

public interface IEntity<out TId> : IEntity
{
    TId Id { get; }
}
