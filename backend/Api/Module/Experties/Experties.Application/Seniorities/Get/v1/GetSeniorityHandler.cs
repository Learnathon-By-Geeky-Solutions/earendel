using Microsoft.Extensions.DependencyInjection;
using TalentMesh.Module.Experties.Domain.Exceptions;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using TalentMesh.Module.Experties.Domain;
using MediatR;

namespace TalentMesh.Module.Experties.Application.Seniorities.Get.v1;
public sealed class GetSeniorityHandler(
    [FromKeyedServices("seniorities:seniorityReadOnly")] IReadRepository<Experties.Domain.Seniority> repository,
    ICacheService cache)
    : IRequestHandler<GetSeniorityRequest, SeniorityResponse>
{
    public async Task<SeniorityResponse> Handle(GetSeniorityRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"seniority:{request.Id}",
            async () =>
            {
                var seniorityItem = await repository.GetByIdAsync(request.Id, cancellationToken);
                if (seniorityItem == null) throw new SeniorityNotFoundException(request.Id);
                return new SeniorityResponse(seniorityItem.Id, seniorityItem.Name, seniorityItem.Description);
            },
            cancellationToken: cancellationToken);
        return item!;
    }
}
