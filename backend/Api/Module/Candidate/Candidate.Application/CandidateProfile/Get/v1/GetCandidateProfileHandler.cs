

using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TalentMesh.Framework.Core.Caching;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Candidate.Domain.Exceptions;

namespace TalentMesh.Module.Candidate.Application.CandidateProfile.Get.v1;

public sealed class GetCandidateProfileHandler(
    [FromKeyedServices("candidate:candidateprofileReadOnly")] IReadRepository<Domain.CandidateProfile> repository,
    ICacheService cache)
    : IRequestHandler<GetCandidateProfileRequest, CandidateProfileResponse>
{
    public async Task<CandidateProfileResponse> Handle(GetCandidateProfileRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"candidateprofile:{request.Id}",
            async () =>
            {
                var candidateProfile = await repository.GetByIdAsync(request.Id, cancellationToken);
                if (candidateProfile == null)
                    throw new CandidateProfileNotFoundException(request.Id);
                return new CandidateProfileResponse(
                    candidateProfile.Id,
                    candidateProfile.Resume!,
                    candidateProfile.Skills!,
                    candidateProfile.Experience!,
                    candidateProfile.Education!,
                    candidateProfile.UserId);
            },
            cancellationToken: cancellationToken);

        return item!;
    }
}

