using TalentMesh.Framework.Core.Tenant.Abstractions;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Tenant.Features.ActivateTenant;
[ExcludeFromCodeCoverage]
public sealed class ActivateTenantHandler(ITenantService service) : IRequestHandler<ActivateTenantCommand, ActivateTenantResponse>
{
    public async Task<ActivateTenantResponse> Handle(ActivateTenantCommand request, CancellationToken cancellationToken)
    {
        var status = await service.ActivateAsync(request.TenantId, cancellationToken);
        return new ActivateTenantResponse(status);
    }
}
