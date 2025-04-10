using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TalentMesh.Module.Experties.Domain.Exceptions;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using TalentMesh.Module.Experties.Domain;
using MediatR;
using TalentMesh.Module.Experties.Application.Seniorities.Get.v1;

namespace TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Get.v1;

public sealed class GetSeniorityLevelJunctionHandler(
    [FromKeyedServices("seniorityleveljunctions:seniorityleveljunctionReadOnly")] IReadRepository<Experties.Domain.SeniorityLevelJunction> repository,
    ICacheService cache,
    ILogger<GetSeniorityLevelJunctionHandler> logger)
    : IRequestHandler<GetSeniorityLevelJunctionRequest, SeniorityLevelJunctionResponse>
{
    public async Task<SeniorityLevelJunctionResponse> Handle(GetSeniorityLevelJunctionRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Handling GetSeniorityLevelJunctionRequest with Id: {RequestId}", request.Id);

        var item = await cache.GetOrSetAsync(
            $"seniorityleveljunction:{request.Id}",
            async () =>
            {
                logger.LogInformation("Cache miss for SeniorityLevelJunction Id: {RequestId}", request.Id);

                var junction = await repository.GetByIdAsync(request.Id, cancellationToken);
                if (junction == null)
                {
                    logger.LogWarning("SeniorityLevelJunction with Id: {RequestId} not found", request.Id);
                    throw new SeniorityLevelJunctionNotFoundException(request.Id);
                }

                logger.LogInformation("SeniorityLevelJunction found: SkillId={SkillId}, SeniorityLevelId={SeniorityLevelId}", junction.SkillId, junction.SeniorityLevelId);

                return new SeniorityLevelJunctionResponse(
                    junction.Id,
                    junction.SeniorityLevelId,
                    junction.SkillId,
                    new SeniorityResponse(
                        junction.Seniority.Id,
                        junction.Seniority.Name,
                        junction.Seniority.Description)
                );
            },
            cancellationToken: cancellationToken);

        logger.LogInformation("Returning SeniorityLevelJunctionResponse for Id: {RequestId}", request.Id);

        return item!;
    }
}
