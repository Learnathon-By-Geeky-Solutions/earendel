using Microsoft.Extensions.DependencyInjection;
using TalentMesh.Module.User.Domain.Exceptions;
using TalentMesh.Framework.Core.Persistence;
using TalentMesh.Framework.Core.Caching;
using TalentMesh.Module.User.Domain;
using MediatR;

namespace FSH.Starter.WebApi.Catalog.Application.Brands.Get.v1;
public sealed class GetBrandHandler(
    [FromKeyedServices("catalog:brands")] IReadRepository<Brand> repository,
    ICacheService cache)
    : IRequestHandler<GetBrandRequest, BrandResponse>
{
    public async Task<BrandResponse> Handle(GetBrandRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"brand:{request.Id}",
            async () =>
            {
                var brandItem = await repository.GetByIdAsync(request.Id, cancellationToken);
                if (brandItem == null) throw new BrandNotFoundException(request.Id);
                return new BrandResponse(brandItem.Id, brandItem.Name, brandItem.Description);
            },
            cancellationToken: cancellationToken);
        return item!;
    }
}
