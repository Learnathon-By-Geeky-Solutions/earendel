using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Tenant.Features.UpgradeSubscription;
[ExcludeFromCodeCoverage]
public record UpgradeSubscriptionResponse(DateTime NewValidity, string Tenant);
