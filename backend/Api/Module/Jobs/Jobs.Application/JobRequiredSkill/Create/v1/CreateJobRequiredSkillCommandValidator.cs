using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace TalentMesh.Module.Job.Application.JobRequiredSkill.Create.v1
{
    [ExcludeFromCodeCoverage]
    public class CreateJobRequiredSkillCommandValidator : AbstractValidator<CreateJobRequiredSkillCommand>
    {
        public CreateJobRequiredSkillCommandValidator()
        {
            RuleFor(x => x.JobId).NotEmpty();
            RuleFor(x => x.SkillId).NotEmpty();
        }
    }
}
