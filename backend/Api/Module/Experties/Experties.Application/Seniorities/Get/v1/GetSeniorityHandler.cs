using Microsoft.Extensions.DependencyInjection;
using TalentMesh.Module.Experties.Domain.Exceptions;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using TalentMesh.Module.Experties.Domain;
using MediatR;

namespace TalentMesh.Module.Experties.Application.Seniorities.Get.v1;

public sealed class GetSeniorityHandler(
    [FromKeyedServices("seniorities:seniorityReadOnly")] IReadRepository<Seniority> repository,
    ICacheService cache
) : IRequestHandler<GetSeniorityRequest, SeniorityResponse>
{
    public async Task<SeniorityResponse> Handle(GetSeniorityRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await cache.GetOrSetAsync(
            $"seniority:{request.Id}",
            async () =>
            {
                var entity = await repository.GetByIdAsync(request.Id, cancellationToken)
                              ?? throw new SeniorityNotFoundException(request.Id);

                return new SeniorityResponse(
                    entity.Id,
                    entity.Name,
                    entity.Description ?? string.Empty
                );
            },
            cancellationToken: cancellationToken
        );

        return item!;
    }
}
