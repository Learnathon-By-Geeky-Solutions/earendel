using MediatR;

namespace TalentMesh.Module.Candidate.Application.CandidateProfile.Update.v1
{
    public sealed record UpdateCandidateProfileCommand(
        Guid Id,
        string Resume,
        string Skills,
        string Experience,
        string Education
        ) : IRequest<UpdateCandidateProfileResponse>;
        
    
}
