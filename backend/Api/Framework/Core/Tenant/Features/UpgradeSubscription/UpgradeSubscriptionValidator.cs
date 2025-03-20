using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Tenant.Features.UpgradeSubscription;
[ExcludeFromCodeCoverage]
public class UpgradeSubscriptionValidator : AbstractValidator<UpgradeSubscriptionCommand>
{
    public UpgradeSubscriptionValidator()
    {
        RuleFor(t => t.Tenant).NotEmpty();
        RuleFor(t => t.ExtendedExpiryDate).GreaterThan(DateTime.UtcNow);
    }
}
