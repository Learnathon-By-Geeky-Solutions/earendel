using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Application.SubSkills.Update.v1;
[ExcludeFromCodeCoverage]
public class UpdateSubSkillCommandValidator : AbstractValidator<UpdateSubSkillCommand>
{
    public UpdateSubSkillCommandValidator()
    {
        RuleFor(b => b.Name).NotEmpty().MinimumLength(2).MaximumLength(100);
        RuleFor(b => b.Description).MaximumLength(1000);
    }
}
