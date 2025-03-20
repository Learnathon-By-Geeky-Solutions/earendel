using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Interviews.Application.InterviewQuestions.Get.v1;
using TalentMesh.Module.Interviews.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Application.InterviewQuestions.Search.v1;

[ExcludeFromCodeCoverage]
public sealed class SearchInterviewQuestionsHandler(
    [FromKeyedServices("interviewquestions:interviewquestionReadOnly")] IReadRepository<InterviewQuestion> repository)
    : IRequestHandler<SearchInterviewQuestionsCommand, PagedList<InterviewQuestionResponse>>
{
    public async Task<PagedList<InterviewQuestionResponse>> Handle(SearchInterviewQuestionsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Create a specification based on the search criteria
        var spec = new SearchInterviewQuestionSpecs(request);

        // List of interviews matching the specification (filters)
        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);

        // Count of total matching interviews
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        // Return a paginated list
        return new PagedList<InterviewQuestionResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
