using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Job.Domain;
using TalentMesh.Module.Job.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Starter.WebApi.Catalog.Application.Brands.Update.v1;
public sealed class UpdateBrandHandler(
    ILogger<UpdateBrandHandler> logger,
    [FromKeyedServices("job:jobs")] IRepository<Jobs> repository)
    : IRequestHandler<UpdateBrandCommand, UpdateBrandResponse>
{
    public async Task<UpdateBrandResponse> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var brand = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = brand ?? throw new JobNotFoundException(request.Id);
        var updatedBrand = brand.Update(request.Name, request.Description);
        await repository.UpdateAsync(updatedBrand, cancellationToken);
        logger.LogInformation("Brand with id : {BrandId} updated.", brand.Id);
        return new UpdateBrandResponse(brand.Id);
    }
}
