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
        var user = Job.Domain.Jobs.Create(request.Name!, request.Description, request.Requirments!, request.Location!,request.JobType!,request.ExperienceLevel!, request.Salary!);
        await repository.AddAsync(user, cancellationToken);
        logger.LogInformation("user created {UserId}", user.Id);
        return new CreateJobResponse(user.Id);
    }
}
