using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Module.Experties.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Create.v1;
public sealed class CreateSeniorityLevelJunctionHandler(
    ILogger<CreateSeniorityLevelJunctionHandler> logger,
    [FromKeyedServices("seniorityleveljunctions:seniorityleveljunction")] IRepository<Experties.Domain.SeniorityLevelJunction> repository)
    : IRequestHandler<CreateSeniorityLevelJunctionCommand, CreateSeniorityLevelJunctionResponse>
{
    public async Task<CreateSeniorityLevelJunctionResponse> Handle(CreateSeniorityLevelJunctionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var seniorityLevelJunction = Experties.Domain.SeniorityLevelJunction.Create(request.SeniorityLevelId, request.SkillId);
        await repository.AddAsync(seniorityLevelJunction, cancellationToken);
        logger.LogInformation("Seniority Level Junction created {SeniorityLevelJunctionId}", seniorityLevelJunction.Id);
        return new CreateSeniorityLevelJunctionResponse(seniorityLevelJunction.Id);
    }
}

