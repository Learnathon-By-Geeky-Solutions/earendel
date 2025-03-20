using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Create.v1;

[ExcludeFromCodeCoverage]

public class CreateSeniorityLevelJunctionCommandValidator : AbstractValidator<CreateSeniorityLevelJunctionCommand>
{
    public CreateSeniorityLevelJunctionCommandValidator()
    {
        RuleFor(b => b.SeniorityLevelId).NotEmpty();
        RuleFor(b => b.SkillId).NotEmpty();
    }
}

