using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Identity.Users.Features.ChangePassword;
[ExcludeFromCodeCoverage]
public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordValidator()
    {
        RuleFor(p => p.Password)
            .NotEmpty();

        RuleFor(p => p.NewPassword)
            .NotEmpty();

        RuleFor(p => p.ConfirmNewPassword)
            .Equal(p => p.NewPassword)
                .WithMessage("passwords do not match.");
    }
}
