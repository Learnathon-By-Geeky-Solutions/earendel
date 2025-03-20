using TalentMesh.Framework.Core.Tenant.Abstractions;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Tenant.Features.CreateTenant;
[ExcludeFromCodeCoverage]
public sealed class CreateTenantHandler(ITenantService service) : IRequestHandler<CreateTenantCommand, CreateTenantResponse>
{
    public async Task<CreateTenantResponse> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        var tenantId = await service.CreateAsync(request, cancellationToken);
        return new CreateTenantResponse(tenantId);
    }
}
