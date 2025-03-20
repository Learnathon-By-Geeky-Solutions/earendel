using TalentMesh.Framework.Core.Tenant.Abstractions;
using TalentMesh.Framework.Core.Tenant.Dtos;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Tenant.Features.GetTenants;
[ExcludeFromCodeCoverage]
public sealed class GetTenantsHandler(ITenantService service) : IRequestHandler<GetTenantsQuery, List<TenantDetail>>
{
    public Task<List<TenantDetail>> Handle(GetTenantsQuery request, CancellationToken cancellationToken)
    {
        return service.GetAllAsync();
    }
}
