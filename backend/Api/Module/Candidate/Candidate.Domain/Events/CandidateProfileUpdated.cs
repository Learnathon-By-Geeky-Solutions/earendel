
using TalentMesh.Framework.Core.Domain.Events;

namespace TalentMesh.Module.Candidate.Domain.Events;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]

public sealed record CandidateProfileUpdated : DomainEvent
{
    public CandidateProfile? CandidateProfile { get; set; }
}

