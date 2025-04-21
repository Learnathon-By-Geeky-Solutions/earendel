using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Experties.Domain;
using TalentMesh.Module.Experties.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Experties.Application.Seniorities.Delete.v1;
public sealed class DeleteSeniorityHandler(
    ILogger<DeleteSeniorityHandler> logger,
    [FromKeyedServices("seniorities:seniority")] IRepository<Experties.Domain.Seniority> repository)
    : IRequestHandler<DeleteSeniorityCommand>
{
    public async Task Handle(DeleteSeniorityCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var seniority = await repository.GetByIdAsync(request.Id, cancellationToken)
                     ?? throw new SeniorityNotFoundException(request.Id);

        await repository.DeleteAsync(seniority, cancellationToken);
        logger.LogInformation("Seniority with id : {SeniorityId} deleted", seniority.Id);
    }
}
