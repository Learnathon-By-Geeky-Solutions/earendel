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

        // Create a list to store created junction IDs
        var createdJunctionIds = new List<Guid>();

        foreach (var seniorityLevelId in request.SeniorityLevelIds)
        {
            var seniorityLevelJunction = Experties.Domain.SeniorityLevelJunction.Create(seniorityLevelId, request.SkillId);
            await repository.AddAsync(seniorityLevelJunction, cancellationToken);
            createdJunctionIds.Add(seniorityLevelJunction.Id);

            logger.LogInformation("Seniority Level Junction created {SeniorityLevelJunctionId}", seniorityLevelJunction.Id);
        }

        return new CreateSeniorityLevelJunctionResponse(createdJunctionIds);
    }
}
