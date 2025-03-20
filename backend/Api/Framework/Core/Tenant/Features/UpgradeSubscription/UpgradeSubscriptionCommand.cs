using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Tenant.Features.UpgradeSubscription;
[ExcludeFromCodeCoverage]
public class UpgradeSubscriptionCommand : IRequest<UpgradeSubscriptionResponse>
{
    public string Tenant { get; set; } = default!;
    public DateTime ExtendedExpiryDate { get; set; }
}
