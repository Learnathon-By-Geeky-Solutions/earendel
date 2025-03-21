using TalentMesh.Framework.Core.Domain.Events;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Domain.Events;
[ExcludeFromCodeCoverage]

public sealed record SeniorityUpdated : DomainEvent
{
    public Seniority? Seniority { get; set; }
}
