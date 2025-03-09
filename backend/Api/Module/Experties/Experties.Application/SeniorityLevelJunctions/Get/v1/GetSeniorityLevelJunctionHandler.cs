using Microsoft.Extensions.DependencyInjection;
using TalentMesh.Module.Experties.Domain.Exceptions;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using TalentMesh.Module.Experties.Domain;
using MediatR;

namespace TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Get.v1;
public sealed class GetSeniorityLevelJunctionHandler(
    [FromKeyedServices("seniorityleveljunctions:seniorityleveljunctionReadOnly")] IReadRepository<Experties.Domain.SeniorityLevelJunction> repository,
    ICacheService cache)
    : IRequestHandler<GetSeniorityLevelJunctionRequest, SeniorityLevelJunctionResponse>
{
    public async Task<SeniorityLevelJunctionResponse> Handle(GetSeniorityLevelJunctionRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"seniorityleveljunction:{request.Id}",
            async () =>
            {
                var junction = await repository.GetByIdAsync(request.Id, cancellationToken);
                if (junction == null || junction.DeletedBy != Guid.Empty)
                    throw new SeniorityLevelJunctionNotFoundException(request.Id);

                return new SeniorityLevelJunctionResponse(
                    junction.Id,
                    junction.SeniorityLevelId,
                    junction.SkillId);
            },
            cancellationToken: cancellationToken);
        return item!;
    }
}