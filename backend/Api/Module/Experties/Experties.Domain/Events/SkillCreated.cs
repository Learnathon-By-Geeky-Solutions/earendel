using TalentMesh.Framework.Core.Domain.Events;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Domain.Events;
[ExcludeFromCodeCoverage]
public sealed record SkillCreated : DomainEvent
{
    public Skill? Skill { get; set; }
}
