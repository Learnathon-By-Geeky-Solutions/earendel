using TalentMesh.Framework.Core.Tenant.Abstractions;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Tenant.Features.UpgradeSubscription;
[ExcludeFromCodeCoverage]

public class UpgradeSubscriptionHandler : IRequestHandler<UpgradeSubscriptionCommand, UpgradeSubscriptionResponse>
{
    private readonly ITenantService _tenantService;

    public UpgradeSubscriptionHandler(ITenantService tenantService) => _tenantService = tenantService;

    public async Task<UpgradeSubscriptionResponse> Handle(UpgradeSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var validUpto = await _tenantService.UpgradeSubscription(request.Tenant, request.ExtendedExpiryDate);
        return new UpgradeSubscriptionResponse(validUpto, request.Tenant);
    }
}
