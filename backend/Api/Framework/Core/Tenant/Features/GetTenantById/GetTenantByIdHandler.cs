using TalentMesh.Framework.Core.Tenant.Abstractions;
using TalentMesh.Framework.Core.Tenant.Dtos;
using MediatR;

namespace TalentMesh.Framework.Core.Tenant.Features.GetTenantById;
public sealed class GetTenantByIdHandler(ITenantService service) : IRequestHandler<GetTenantByIdQuery, TenantDetail>
{
    public async Task<TenantDetail> Handle(GetTenantByIdQuery request, CancellationToken cancellationToken)
    {
        return await service.GetByIdAsync(request.TenantId);
    }
}
