using TalentMesh.Framework.Core.Tenant.Abstractions;
using TalentMesh.Framework.Core.Tenant.Dtos;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Tenant.Features.GetTenantById;
[ExcludeFromCodeCoverage]
public sealed class GetTenantByIdHandler(ITenantService service) : IRequestHandler<GetTenantByIdQuery, TenantDetail>
{
    public async Task<TenantDetail> Handle(GetTenantByIdQuery request, CancellationToken cancellationToken)
    {
        return await service.GetByIdAsync(request.TenantId);
    }
}
