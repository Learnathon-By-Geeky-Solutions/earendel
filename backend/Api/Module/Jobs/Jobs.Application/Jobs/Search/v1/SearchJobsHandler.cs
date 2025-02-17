using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Job.Application.Jobs.Get.v1;
using TalentMesh.Module.Job.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace TalentMesh.Module.Job.Application.Jobs.Search.v1;
public sealed class SearchJobsHandler(
    [FromKeyedServices("catalog:brands")] IReadRepository<Job.Domain.Jobs> repository)
    : IRequestHandler<SearchJobsCommand, PagedList<JobResponse>>
{
    public async Task<PagedList<JobResponse>> Handle(SearchJobsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchJobSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<JobResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}
