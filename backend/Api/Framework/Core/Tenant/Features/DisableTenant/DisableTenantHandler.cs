using TalentMesh.Framework.Core.Tenant.Abstractions;
using MediatR;

namespace TalentMesh.Framework.Core.Tenant.Features.DisableTenant;
public sealed class DisableTenantHandler(ITenantService service) : IRequestHandler<DisableTenantCommand, DisableTenantResponse>
{
    public async Task<DisableTenantResponse> Handle(DisableTenantCommand request, CancellationToken cancellationToken)
    {
        var status = await service.DeactivateAsync(request.TenantId);
        return new DisableTenantResponse(status);
    }
}
