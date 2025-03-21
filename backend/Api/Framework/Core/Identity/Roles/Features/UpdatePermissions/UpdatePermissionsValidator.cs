using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Identity.Roles.Features.UpdatePermissions;
[ExcludeFromCodeCoverage]
public class UpdatePermissionsValidator : AbstractValidator<UpdatePermissionsCommand>
{
    public UpdatePermissionsValidator()
    {
        RuleFor(r => r.RoleId)
            .NotEmpty();
        RuleFor(r => r.Permissions)
            .NotNull();
    }
}
