using TalentMesh.Framework.Core.Domain.Events;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Domain.Events;
[ExcludeFromCodeCoverage]
public sealed record InterviewUpdated : DomainEvent
{
    public Interview? Interview { get; set; }
}
