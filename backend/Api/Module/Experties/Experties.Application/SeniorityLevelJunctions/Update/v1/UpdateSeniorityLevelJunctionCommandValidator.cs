using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Update.v1
{
    [ExcludeFromCodeCoverage]
    public class UpdateSeniorityLevelJunctionCommandValidator : AbstractValidator<UpdateSeniorityLevelJunctionCommand>
    {
        public UpdateSeniorityLevelJunctionCommandValidator()
        {
            RuleFor(x => x.SkillId)
                .NotEmpty().WithMessage("SkillId is required.");

            RuleFor(x => x.SeniorityLevelIds)
                .NotNull().WithMessage("SeniorityLevelIds cannot be null.")
                .Must(ids => ids.Count > 0)
                .WithMessage("At least one SeniorityLevelId must be provided.");
        }
    }
}
