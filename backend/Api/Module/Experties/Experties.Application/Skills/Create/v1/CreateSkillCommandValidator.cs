using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Application.Skills.Create.v1;

[ExcludeFromCodeCoverage]
public class CreateSkillCommandValidator : AbstractValidator<CreateSkillCommand>
{
    public CreateSkillCommandValidator()
    {
        RuleFor(b => b.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100);

        RuleFor(b => b.Description)
            .MaximumLength(1000);

        RuleFor(b => b.SeniorityLevels)
            .NotNull().WithMessage("Seniority levels are required.")
            .Must(levels => levels.Count > 0)
            .WithMessage("At least one seniority level must be provided.");

        RuleForEach(b => b.SeniorityLevels)
            .NotEmpty().WithMessage("Each seniority level must be a valid GUID.");
    }
}
