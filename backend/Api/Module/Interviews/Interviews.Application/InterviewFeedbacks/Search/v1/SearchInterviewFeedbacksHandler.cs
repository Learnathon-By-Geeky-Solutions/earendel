using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Interviews.Domain;
using TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Get.v1;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Search.v1;

public sealed class SearchInterviewFeedbacksHandler(
    [FromKeyedServices("interviewfeedbacks:interviewfeedbackReadOnly")] IReadRepository<InterviewFeedback> repository)
    : IRequestHandler<SearchInterviewFeedbacksCommand, PagedList<InterviewFeedbackResponse>>
{
    public async Task<PagedList<InterviewFeedbackResponse>> Handle(SearchInterviewFeedbacksCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Create a specification based on the search criteria
        var spec = new SearchInterviewFeedbackSpecs(request);

        // List of interviews matching the specification (filters)
        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);

        // Count of total matching interviews
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        // Return a paginated list
        return new PagedList<InterviewFeedbackResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
