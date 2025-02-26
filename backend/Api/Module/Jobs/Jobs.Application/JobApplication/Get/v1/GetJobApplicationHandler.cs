using Microsoft.Extensions.DependencyInjection;
using TalentMesh.Module.Job.Domain.Exceptions;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using TalentMesh.Module.Job.Domain;
using MediatR;

namespace TalentMesh.Module.Job.Application.JobApplication.Get.v1;
public sealed class GetJobApplicationHandler(
    [FromKeyedServices("jobs:jobApplicationReadOnly")] IReadRepository<Domain.JobApplication> repository,
    ICacheService cache)
    : IRequestHandler<GetJobApplicationRequest, JobApplicationResponse>
{
    public async Task<JobApplicationResponse> Handle(GetJobApplicationRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"brand:{request.Id}",
            async () =>
            {
                var brandItem = await repository.GetByIdAsync(request.Id, cancellationToken);
                if (brandItem == null) throw new JobNotFoundException(request.Id);
                return new JobApplicationResponse(brandItem.Id, brandItem.JobId, brandItem.CandidateId, brandItem.ApplicationDate, brandItem.Status, brandItem.CoverLetter);
            },
            cancellationToken: cancellationToken);
        return item!;
    }
}
