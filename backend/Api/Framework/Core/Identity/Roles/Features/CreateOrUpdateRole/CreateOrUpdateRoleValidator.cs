using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Identity.Roles.Features.CreateOrUpdateRole;
[ExcludeFromCodeCoverage]
public class CreateOrUpdateRoleValidator : AbstractValidator<CreateOrUpdateRoleCommand>
{
    public CreateOrUpdateRoleValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Role name is required.");
    }
}
