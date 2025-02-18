using MediatR;

namespace TalentMesh.Framework.Core.Tenant.Features.UpgradeSubscription;
public class UpgradeSubscriptionCommand : IRequest<UpgradeSubscriptionResponse>
{
    public string Tenant { get; set; } = default!;
    public DateTime ExtendedExpiryDate { get; set; }
}
