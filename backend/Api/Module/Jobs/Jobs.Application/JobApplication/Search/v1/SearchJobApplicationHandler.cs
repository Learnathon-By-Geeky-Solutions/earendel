using TalentMesh.Framework.Core.Paging;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Job.Application.JobApplication.Get.v1;
using TalentMesh.Module.Job.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace TalentMesh.Module.Job.Application.JobApplication.Search.v1;
public sealed class SearchJobApplicationHandler(
    [FromKeyedServices("jobs:jobApplicationReadOnly")] IReadRepository<Domain.JobApplication> repository)
    : IRequestHandler<SearchJobApplicationsCommand, PagedList<JobApplicationResponse>>
{
    public async Task<PagedList<JobApplicationResponse>> Handle(SearchJobApplicationsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchJobApplicationSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<JobApplicationResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}
