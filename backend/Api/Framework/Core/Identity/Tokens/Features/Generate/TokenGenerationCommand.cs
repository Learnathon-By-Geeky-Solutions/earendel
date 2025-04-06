using System.ComponentModel;
using FluentValidation;
using TalentMesh.Shared.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Core.Identity.Tokens.Features.Generate;
[ExcludeFromCodeCoverage]
public record TokenGenerationCommand(
    [property: DefaultValue(TenantConstants.Root.EmailAddress)] string Email,
    [property: DefaultValue(TenantConstants.DefaultPassword)] string? Password);

[ExcludeFromCodeCoverage]

public class GenerateTokenValidator : AbstractValidator<TokenGenerationCommand>
{
    public GenerateTokenValidator()
    {
        RuleFor(p => p.Email).Cascade(CascadeMode.Stop).NotEmpty().EmailAddress();

        When(p => p.Password is not null, () =>
            {
                RuleFor(p => p.Password)
                    .NotEmpty();
            });
    }
}
