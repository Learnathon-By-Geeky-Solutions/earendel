using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Candidate.Application.CandidateProfile.Get.v1;

namespace TalentMesh.Module.Candidate.Application.CandidateProfile.Search.v1;

[ExcludeFromCodeCoverage]

public sealed class SearchCandidateProfileHandler(
    [FromKeyedServices("candidate:candidateprofileReadOnly")] IReadRepository<Domain.CandidateProfile> repository)
    : IRequestHandler<SearchCandidateProfileCommand, PagedList<CandidateProfileResponse>>
{
    public async Task<PagedList<CandidateProfileResponse>> Handle(SearchCandidateProfileCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchCandidateProfileSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<CandidateProfileResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}
