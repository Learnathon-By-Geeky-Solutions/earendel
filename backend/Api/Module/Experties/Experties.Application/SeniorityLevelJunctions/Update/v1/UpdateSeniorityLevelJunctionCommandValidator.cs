using FluentValidation;

namespace TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Update.v1;
public class UpdateSeniorityLevelJunctionCommandValidator : AbstractValidator<UpdateSeniorityLevelJunctionCommand>
{
    public UpdateSeniorityLevelJunctionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.SeniorityLevelId).NotEmpty();
        RuleFor(x => x.SkillId).NotEmpty();
    }
}