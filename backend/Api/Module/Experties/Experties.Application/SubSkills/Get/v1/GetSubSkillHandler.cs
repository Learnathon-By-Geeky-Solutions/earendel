using Microsoft.Extensions.DependencyInjection;
using TalentMesh.Module.Experties.Domain.Exceptions;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using TalentMesh.Module.Experties.Domain;
using MediatR;

namespace TalentMesh.Module.Experties.Application.SubSkills.Get.v1;
public sealed class GetSubSkillHandler(
    [FromKeyedServices("subskills:subskillReadOnly")] IReadRepository<Experties.Domain.SubSkill> repository,
    ICacheService cache)
    : IRequestHandler<GetSubSkillRequest, SubSkillResponse>
{
    public async Task<SubSkillResponse> Handle(GetSubSkillRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"subskill:{request.Id}",
            async () =>
            {
                var skillItem = await repository.GetByIdAsync(request.Id, cancellationToken);
                if (skillItem == null || skillItem.DeletedBy != Guid.Empty) throw new SubSkillNotFoundException(request.Id);
                return new SubSkillResponse(skillItem.Id, skillItem.Name, skillItem.Description);
            },
            cancellationToken: cancellationToken);
        return item!;
    }
}
