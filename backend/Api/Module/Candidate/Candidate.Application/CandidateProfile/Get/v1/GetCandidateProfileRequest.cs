using MediatR;
namespace TalentMesh.Module.Candidate.Application.CandidateProfile.Get.v1;

public sealed class GetCandidateProfileRequest : IRequest<CandidateProfileResponse>
{
    public Guid Id { get; set; }
    public GetCandidateProfileRequest(Guid id) => Id = id;
}


