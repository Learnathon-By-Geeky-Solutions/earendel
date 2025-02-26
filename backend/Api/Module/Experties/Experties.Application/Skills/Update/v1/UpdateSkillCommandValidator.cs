using FluentValidation;

namespace TalentMesh.Module.Experties.Application.Skills.Update.v1;
public class UpdateSkillCommandValidator : AbstractValidator<UpdateSkillCommand>
{
    public UpdateSkillCommandValidator()
    {
        RuleFor(b => b.Name).NotEmpty().MinimumLength(2).MaximumLength(100);
        RuleFor(b => b.Description).MaximumLength(1000);
    }
}
