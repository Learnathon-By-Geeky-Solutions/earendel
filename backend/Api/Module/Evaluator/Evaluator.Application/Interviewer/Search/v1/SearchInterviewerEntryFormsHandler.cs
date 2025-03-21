using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Evaluator.Application.Interviewer.Get.v1;
using TalentMesh.Module.Evaluator.Application.Interviewer.Search.v1;
using TalentMesh.Module.Evaluator.Domain;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Search.v1
{
    [ExcludeFromCodeCoverage]
    public sealed class SearchInterviewerEntryFormsHandler(
        [FromKeyedServices("interviews:interviewerentryformReadOnly")] IReadRepository<InterviewerEntryForm> repository)
        : IRequestHandler<SearchInterviewerEntryFormsCommand, PagedList<InterviewerEntryFormResponse>>
    {
        public async Task<PagedList<InterviewerEntryFormResponse>> Handle(SearchInterviewerEntryFormsCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var spec = new SearchInterviewerEntryFormSpecs(request);

            var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
            var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

            return new PagedList<InterviewerEntryFormResponse>(items, request.PageNumber, request.PageSize, totalCount);
        }
    }
}
