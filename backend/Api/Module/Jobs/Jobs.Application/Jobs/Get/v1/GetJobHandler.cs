using Microsoft.Extensions.DependencyInjection;
using TalentMesh.Module.Job.Domain.Exceptions;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using TalentMesh.Module.Job.Domain;
using MediatR;

namespace TalentMesh.Module.Job.Application.Jobs.Get.v1;
public sealed class GetJobHandler(
    [FromKeyedServices("jobs:jobReadOnly")] IReadRepository<Job.Domain.Jobs> repository,
    ICacheService cache)
    : IRequestHandler<GetJobRequest, JobResponse>
{
    public async Task<JobResponse> Handle(GetJobRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"jobs:{request.Id}",
            async () =>
            {
                var brandItem = await repository.GetByIdAsync(request.Id, cancellationToken);
                if (brandItem == null) throw new JobNotFoundException(request.Id);
                return new JobResponse(brandItem.Id, brandItem.Name, brandItem.Description, 
                    brandItem.Requirments, brandItem.Location, brandItem.JobType, brandItem.ExperienceLevel
                    );
            },
            cancellationToken: cancellationToken);
        return item!;
    }
}
