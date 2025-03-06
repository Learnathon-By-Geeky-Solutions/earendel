using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Interviews.Application.Interviews.Get.v1;
using TalentMesh.Module.Interviews.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace TalentMesh.Module.Interviews.Application.Interviews.Search.v1;

public sealed class SearchInterviewsHandler(
    [FromKeyedServices("interviews:interviewReadOnly")] IReadRepository<Interview> repository)
    : IRequestHandler<SearchInterviewsCommand, PagedList<InterviewResponse>>
{
    public async Task<PagedList<InterviewResponse>> Handle(SearchInterviewsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Create a specification based on the search criteria
        var spec = new SearchInterviewSpecs(request);

        // List of interviews matching the specification (filters)
        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);

        // Count of total matching interviews
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        // Return a paginated list
        return new PagedList<InterviewResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
