using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Job.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Job.Application.Jobs.Create.v1;
public sealed class CreateJobApplicationHandler(
    ILogger<CreateJobApplicationHandler> logger,
    [FromKeyedServices("jobs:job")] IRepository<Job.Domain.Jobs> repository)
    : IRequestHandler<CreateJobCommand, CreateJobResponse>
{
    public async Task<CreateJobResponse> Handle(CreateJobCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var jobInfo = new JobInfo
        {
            Name = request.Name!,
            Description = request.Description,
            Requirments = request.Requirments!,
            Location = request.Location!,
            JobType = request.JobType!,
            ExperienceLevel = request.ExperienceLevel!,
            Salary = request.Salary!,
            PostedById = request.PostedById
        };

        var user = Job.Domain.Jobs.Create(jobInfo);
        await repository.AddAsync(user, cancellationToken);
        logger.LogInformation("job created {UserId}", user.Id);
        return new CreateJobResponse(user.Id);
    }
}
