using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

namespace TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Create.v1;

[ExcludeFromCodeCoverage]
public class CreateSeniorityLevelJunctionCommandValidator : AbstractValidator<CreateSeniorityLevelJunctionCommand>
{
    public CreateSeniorityLevelJunctionCommandValidator()
    {
        RuleFor(b => b.SkillId).NotEmpty();
        RuleFor(b => b.SeniorityLevelIds)
            .NotEmpty()
            .Must(seniorityLevels => seniorityLevels.Count > 0)
            .WithMessage("At least one Seniority Level ID must be provided.");
    }
}
