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
    public sealed class SearchInterviewerAvailabilitiesHandler(
        [FromKeyedServices("interviews:intervieweravailabilityReadOnly")] IReadRepository<InterviewerAvailability> repository)
        : IRequestHandler<SearchInterviewerAvailabilitiesCommand, PagedList<InterviewerAvailabilityResponse>>
    {
        public async Task<PagedList<InterviewerAvailabilityResponse>> Handle(SearchInterviewerAvailabilitiesCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var spec = new SearchInterviewerAvailabilitySpecs(request);

            var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
            var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

            return new PagedList<InterviewerAvailabilityResponse>(items, request.PageNumber, request.PageSize, totalCount);
        }
    }
}
