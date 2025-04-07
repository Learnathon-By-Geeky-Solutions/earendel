

namespace TalentMesh.Module.Candidate.Application.CandidateProfile.Update.v1
{
    public sealed record UpdateCandidateProfileResponse(
    Guid? Id,
    string? Resume,
    string? Skills,
    string? Experience,
    string? Education
);

}
