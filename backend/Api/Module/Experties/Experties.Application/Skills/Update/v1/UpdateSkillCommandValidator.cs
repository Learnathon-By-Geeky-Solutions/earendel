using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Experties.Application.Skills.Update.v1
{
    [ExcludeFromCodeCoverage]
    public class UpdateSkillCommandValidator : AbstractValidator<UpdateSkillCommand>
    {
        public UpdateSkillCommandValidator()
        {
            RuleFor(b => b.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters long.")
                .MaximumLength(100).WithMessage("Name must be at most 100 characters long.");

            RuleFor(b => b.Description)
                .MaximumLength(1000).WithMessage("Description must be at most 1000 characters long.");

            // Single rule without branching to validate seniority levels
            RuleFor(x => x.SeniorityLevelIds)
                .Must(levels => levels == null || levels.Count > 0)
                .WithMessage("At least one seniority level must be provided when SeniorityLevelIds is not null.");
        }
    }
}