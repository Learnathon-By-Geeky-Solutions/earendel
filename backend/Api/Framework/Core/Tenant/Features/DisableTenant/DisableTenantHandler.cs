using TalentMesh.Framework.Core.Tenant.Abstractions;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Tenant.Features.DisableTenant;
[ExcludeFromCodeCoverage]
public sealed class DisableTenantHandler(ITenantService service) : IRequestHandler<DisableTenantCommand, DisableTenantResponse>
{
    public async Task<DisableTenantResponse> Handle(DisableTenantCommand request, CancellationToken cancellationToken)
    {
        var status = await service.DeactivateAsync(request.TenantId);
        return new DisableTenantResponse(status);
    }
}
