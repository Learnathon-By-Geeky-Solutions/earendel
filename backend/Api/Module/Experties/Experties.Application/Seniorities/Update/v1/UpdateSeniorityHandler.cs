using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Experties.Domain;
using TalentMesh.Module.Experties.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Experties.Application.Seniorities.Update.v1;
public sealed class UpdateSeniorityHandler(
    ILogger<UpdateSeniorityHandler> logger,
    [FromKeyedServices("seniorities:seniority")] IRepository<Experties.Domain.Seniority> repository)
    : IRequestHandler<UpdateSeniorityCommand, UpdateSeniorityResponse>
{
    public async Task<UpdateSeniorityResponse> Handle(UpdateSeniorityCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var seniority = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (seniority is null || seniority.DeletedBy is not null)
        {
            throw new SeniorityNotFoundException(request.Id);
        }
        var updatedSeniority = seniority.Update(request.Name, request.Description);
        await repository.UpdateAsync(updatedSeniority, cancellationToken);
        logger.LogInformation("seniority with id : {SeniorityId} updated.", seniority.Id);
        return new UpdateSeniorityResponse(updatedSeniority.Id);
    }
}
