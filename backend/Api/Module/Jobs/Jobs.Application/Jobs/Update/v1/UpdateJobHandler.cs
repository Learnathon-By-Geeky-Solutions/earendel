using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Job.Domain;
using TalentMesh.Module.Job.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Hangfire.Common;

namespace TalentMesh.Module.Job.Application.Jobs.Update.v1;

public sealed class UpdateJobHandler(
    ILogger<UpdateJobHandler> logger,
    [FromKeyedServices("jobs:job")] IRepository<Job.Domain.Jobs> repository)
    : IRequestHandler<UpdateJobCommand, UpdateJobResponse>
{
    public async Task<UpdateJobResponse> Handle(UpdateJobCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var brand = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (brand is null)
        {
            throw new JobNotFoundException(request.Id);
        }

        var jobUpdateDetails = new JobUpdateDetails
        {
            Name = request.Name,
            Description = request.Description,
            Requirments = request.Requirments,
            Location = request.Location,
            JobType = request.JobType,
            ExperienceLevel = request.ExperienceLevel,
            Salary = request.Salary
        };


        var updatedBrand = brand.Update(jobUpdateDetails);

        await repository.UpdateAsync(updatedBrand, cancellationToken);

        logger.LogInformation("Brand with id : {BrandId} updated.", brand.Id);
        return new UpdateJobResponse(brand.Id);
    }
}
