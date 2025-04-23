using Microsoft.Extensions.DependencyInjection;
using TalentMesh.Module.Experties.Domain.Exceptions;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using TalentMesh.Module.Experties.Domain;
using MediatR;

namespace TalentMesh.Module.Experties.Application.Rubrics.Get.v1
{
    public sealed class GetRubricHandler(
        [FromKeyedServices("rubrics:rubricReadOnly")] IReadRepository<Rubric> repository,
        ICacheService cache
    ) : IRequestHandler<GetRubricRequest, RubricResponse>
    {
        public async Task<RubricResponse> Handle(GetRubricRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var item = await cache.GetOrSetAsync(
                $"rubric:{request.Id}",
                async () =>
                {
                    // Null-coalescing throw replaces the 'if' check
                    var rubricEntity = await repository
                        .GetByIdAsync(request.Id, cancellationToken)
                        .ConfigureAwait(false)
                        ?? throw new RubricNotFoundException(request.Id);

                    return new RubricResponse(
                        rubricEntity.Id,
                        rubricEntity.Title,
                        rubricEntity.RubricDescription,
                        rubricEntity.SubSkillId,
                        rubricEntity.SeniorityId,
                        rubricEntity.Weight
                    );
                },
                cancellationToken: cancellationToken
            );

            return item!;
        }
    }
}
