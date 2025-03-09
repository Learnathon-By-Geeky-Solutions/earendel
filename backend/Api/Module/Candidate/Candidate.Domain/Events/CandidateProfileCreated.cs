
using TalentMesh.Framework.Core.Domain.Events;

namespace TalentMesh.Module.Candidate.Domain.Events;

public sealed record CandidateProfileCreated : DomainEvent
{
    public CandidateProfile? CandidateProfile { get; set; }
}

