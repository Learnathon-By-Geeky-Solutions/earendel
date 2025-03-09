using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Experties.Domain;
using TalentMesh.Module.Experties.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Update.v1;
public sealed class UpdateSeniorityLevelJunctionHandler(
    ILogger<UpdateSeniorityLevelJunctionHandler> logger,
    [FromKeyedServices("seniorityleveljunctions:seniorityleveljunction")] IRepository<Experties.Domain.SeniorityLevelJunction> repository)
    : IRequestHandler<UpdateSeniorityLevelJunctionCommand, UpdateSeniorityLevelJunctionResponse>
{
    public async Task<UpdateSeniorityLevelJunctionResponse> Handle(UpdateSeniorityLevelJunctionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var junction = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (junction is null || junction.DeletedBy != Guid.Empty)
        {
            throw new SeniorityLevelJunctionNotFoundException(request.Id);
        }

        var updatedJunction = junction.Update(request.SeniorityLevelId, request.SkillId);
        await repository.UpdateAsync(updatedJunction, cancellationToken);

        logger.LogInformation("SeniorityLevelJunction with id: {JunctionId} updated.", junction.Id);
        return new UpdateSeniorityLevelJunctionResponse(updatedJunction.Id);
    }
}