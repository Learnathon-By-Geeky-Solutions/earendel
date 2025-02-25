using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Job.Domain;
using TalentMesh.Module.Job.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

        if (brand is null || brand.DeletedBy != Guid.Empty)
        {
            throw new JobNotFoundException(request.Id);
        }

        var updatedBrand = brand.Update(request.Name, request.Description);
        await repository.UpdateAsync(updatedBrand, cancellationToken);

        logger.LogInformation("Brand with id : {BrandId} updated: Name='{BrandName}', Description='{BrandDescription}'",
            brand.Id, request.Name, request.Description);

        return new UpdateJobResponse(brand.Id);
    }
}
