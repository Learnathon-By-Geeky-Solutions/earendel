using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Job.Application.JobRequiredSubskill.Get.v1;
using TalentMesh.Module.Job.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace TalentMesh.Module.Job.Application.JobRequiredSubskill.Search.v1
{
    public sealed class SearchJobRequiredSubskillHandler(
        [FromKeyedServices("jobs:jobrequiredsubskillReadOnly")] IReadRepository<Domain.JobRequiredSubskill> repository)
        : IRequestHandler<SearchJobRequiredSubskillCommand, PagedList<JobRequiredSubskillResponse>>
    {
        public async Task<PagedList<JobRequiredSubskillResponse>> Handle(SearchJobRequiredSubskillCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var spec = new SearchJobRequiredSubskillSpecs(request);
            var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
            var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);
            return new PagedList<JobRequiredSubskillResponse>(items, request.PageNumber, request.PageSize, totalCount);
        }
    }
}
