using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Application.SubSkills.Create.v1;

[ExcludeFromCodeCoverage]

public class CreateSubSkillCommandValidator : AbstractValidator<CreateSubSkillCommand>
{
    public CreateSubSkillCommandValidator()
    {
        RuleFor(b => b.Name).NotEmpty().MinimumLength(2).MaximumLength(100);
        RuleFor(b => b.Description).MaximumLength(1000);
    }
}
