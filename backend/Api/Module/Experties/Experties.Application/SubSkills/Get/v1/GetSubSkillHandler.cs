using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TalentMesh.Module.Experties.Domain.Exceptions;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using TalentMesh.Module.Experties.Domain;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace TalentMesh.Module.Experties.Application.SubSkills.Get.v1;

public sealed class GetSubSkillHandler(
    [FromKeyedServices("subskills:subskillReadOnly")] IReadRepository<SubSkill> repository,
    ICacheService cache
) : IRequestHandler<GetSubSkillRequest, SubSkillResponse>
{
    public async Task<SubSkillResponse> Handle(GetSubSkillRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await cache.GetOrSetAsync(
            $"subskill:{request.Id}",
            async () =>
            {
                var entity = await repository.GetByIdAsync(request.Id, cancellationToken)
                               ?? throw new SubSkillNotFoundException(request.Id);

                return new SubSkillResponse(
                    entity.Id,
                    entity.Name,
                    entity.Description,
                    entity.SkillId
                );
            },
            cancellationToken: cancellationToken
        );

        return item!;
    }
}
