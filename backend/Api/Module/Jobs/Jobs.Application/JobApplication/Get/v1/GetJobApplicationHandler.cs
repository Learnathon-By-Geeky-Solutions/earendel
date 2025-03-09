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
            $"jobApplication:{request.Id}",
            async () =>
            {
                var jobApplication = await repository.GetByIdAsync(request.Id, cancellationToken);
                if (jobApplication == null || jobApplication.DeletedBy != Guid.Empty)
                    throw new JobApplicationNotFoundException(request.Id);
                return new JobApplicationResponse(jobApplication.Id, 
                    jobApplication.JobId, jobApplication.CandidateId, 
                    jobApplication.ApplicationDate, jobApplication.Status, 
                    jobApplication.CoverLetter);
            },
            cancellationToken: cancellationToken);
        return item!;
    }
}
