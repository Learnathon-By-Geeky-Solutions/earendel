using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Job.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Job.Application.JobApplication.Create.v1;
public sealed class CreateJobApplicationHandler(
    ILogger<CreateJobApplicationHandler> logger,
    [FromKeyedServices("jobs:jobapplication")] IRepository<Domain.JobApplication> repository)
    : IRequestHandler<CreateJobApplicationCommand, CreateJobApplicationResponse>
{
    public async Task<CreateJobApplicationResponse> Handle(CreateJobApplicationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var jobApplication = Domain.JobApplication.Create(
            request.JobId!,
            request.CandidateId!,
            request.CoverLetter!
        );
        await repository.AddAsync(jobApplication, cancellationToken);
        logger.LogInformation("JobApplication Created {UserId}", jobApplication.Id);
        return new CreateJobApplicationResponse(jobApplication.Id);
    }
}
