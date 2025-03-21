using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Tenant.Features.DisableTenant;
[ExcludeFromCodeCoverage]
public sealed class DisableTenantValidator : AbstractValidator<DisableTenantCommand>
{
    public DisableTenantValidator() =>
       RuleFor(t => t.TenantId)
           .NotEmpty();
}
