using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Tenant.Features.ActivateTenant;
[ExcludeFromCodeCoverage]
public sealed class ActivateTenantValidator : AbstractValidator<ActivateTenantCommand>
{
    public ActivateTenantValidator() =>
       RuleFor(t => t.TenantId)
           .NotEmpty();
}
