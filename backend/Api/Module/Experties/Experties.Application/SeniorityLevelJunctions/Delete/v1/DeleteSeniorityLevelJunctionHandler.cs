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
        var SeniorityLevelJunction = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (SeniorityLevelJunction == null || seniorityleveljunction.DeletedBy != Guid.Empty) throw new SeniorityLevelJunctionNotFoundException(request.Id);
        await repository.DeleteAsync(seniorityleveljunction, cancellationToken);
        logger.LogInformation("SeniorityLevelJunction with id : {SeniorityLevelId} deleted", SeniorityLevelJunction.Id);
    }
}
