using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Evaluator.Application.Interviewer.Get.v1;
using TalentMesh.Module.Evaluator.Domain;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Search.v1
{
    [ExcludeFromCodeCoverage]
    public sealed class SearchInterviewerApplicationsHandler(
        [FromKeyedServices("interviews:interviewerapplicationReadOnly")] IReadRepository<InterviewerApplication> repository)
        : IRequestHandler<SearchInterviewerApplicationsCommand, PagedList<InterviewerApplicationResponse>>
    {
        public async Task<PagedList<InterviewerApplicationResponse>> Handle(SearchInterviewerApplicationsCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var spec = new SearchInterviewerApplicationSpecs(request);

            var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
            var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

            return new PagedList<InterviewerApplicationResponse>(items, request.PageNumber, request.PageSize, totalCount);
        }
    }
}
