using Microsoft.Extensions.DependencyInjection;
using TalentMesh.Module.Experties.Domain.Exceptions;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using TalentMesh.Module.Experties.Domain;
using MediatR;

namespace TalentMesh.Module.Experties.Application.Skills.Get.v1;
public sealed class GetSkillHandler(
    [FromKeyedServices("skills:skillReadOnly")] IReadRepository<Experties.Domain.Skill> repository,
    ICacheService cache)
    : IRequestHandler<GetSkillRequest, SkillResponse>
{
    public async Task<SkillResponse> Handle(GetSkillRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"skill:{request.Id}",
            async () =>
            {
                var skillItem = await repository.GetByIdAsync(request.Id, cancellationToken);
                if (skillItem == null) throw new SkillNotFoundException(request.Id);
                return new SkillResponse(skillItem.Id, skillItem.Name, skillItem.Description);
            },
            cancellationToken: cancellationToken);
        return item!;
    }
}
