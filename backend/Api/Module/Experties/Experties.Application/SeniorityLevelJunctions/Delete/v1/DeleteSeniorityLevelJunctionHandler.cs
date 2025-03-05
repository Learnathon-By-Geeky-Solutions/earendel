using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Experties.Domain;
using TalentMesh.Module.Experties.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Delete.v1;
public sealed class DeleteSeniorityLevelJunctionHandler(
    ILogger<DeleteSeniorityLevelJunctionHandler> logger,
    [FromKeyedServices("seniorityleveljunctions:seniorityleveljunction")] IRepository<Experties.Domain.SeniorityLevelJunction> repository)
    : IRequestHandler<DeleteSeniorityLevelJunctionCommand>
{
    public async Task Handle(DeleteSeniorityLevelJunctionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var seniorityLevelJunction = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (seniorityLevelJunction == null || seniorityLevelJunction.DeletedBy != Guid.Empty) throw new SeniorityLevelJunctionNotFoundException(request.Id);
        await repository.DeleteAsync(seniorityLevelJunction, cancellationToken);
        logger.LogInformation("SeniorityLevelJunction with id : {SeniorityLevelId} deleted", seniorityLevelJunction.Id);
    }
}
