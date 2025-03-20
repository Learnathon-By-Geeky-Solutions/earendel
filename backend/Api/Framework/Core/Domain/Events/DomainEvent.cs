using MediatR;
using System.Diagnostics.CodeAnalysis;
namespace TalentMesh.Framework.Core.Domain.Events;
[ExcludeFromCodeCoverage]
public abstract record DomainEvent : IDomainEvent, INotification
{
    public DateTime RaisedOn { get; protected set; } = DateTime.UtcNow;
}
