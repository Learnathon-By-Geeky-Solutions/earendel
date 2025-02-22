using FluentValidation;

namespace TalentMesh.Module.Experties.Application.SubSkills.Update.v1;
public class UpdateSubSkillCommandValidator : AbstractValidator<UpdateSubSkillCommand>
{
    public UpdateSubSkillCommandValidator()
    {
        RuleFor(b => b.Name).NotEmpty().MinimumLength(2).MaximumLength(100);
        RuleFor(b => b.Description).MaximumLength(1000);
    }
}
