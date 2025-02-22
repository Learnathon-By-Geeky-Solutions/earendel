using Microsoft.Extensions.DependencyInjection;
using TalentMesh.Module.Experties.Domain.Exceptions;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using TalentMesh.Module.Experties.Domain;
using MediatR;

namespace TalentMesh.Module.Experties.Application.Rubrics.Get.v1;
public sealed class GetRubricHandler(
    [FromKeyedServices("rubrics:rubricReadOnly")] IReadRepository<Experties.Domain.Rubric> repository,
    ICacheService cache)
    : IRequestHandler<GetRubricRequest, RubricResponse>
{
    public async Task<RubricResponse> Handle(GetRubricRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"rubric:{request.Id}",
            async () =>
            {
                var rubricItem = await repository.GetByIdAsync(request.Id, cancellationToken);
                if (rubricItem == null || rubricItem.DeletedBy != Guid.Empty) throw new RubricNotFoundException(request.Id);
                return new RubricResponse(rubricItem.Id, rubricItem.Title, rubricItem.RubricDescription, rubricItem.SubSkillId, rubricItem.SeniorityLevelId, rubricItem.Weight);
            },
            cancellationToken: cancellationToken);
        return item!;
    }
}
