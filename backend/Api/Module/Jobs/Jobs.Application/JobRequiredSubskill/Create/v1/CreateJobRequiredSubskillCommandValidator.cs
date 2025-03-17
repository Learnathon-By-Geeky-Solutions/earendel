using FluentValidation;

namespace TalentMesh.Module.Job.Application.JobRequiredSubskill.Create.v1
{
    public class CreateJobRequiredSubskillCommandValidator : AbstractValidator<CreateJobRequiredSubskillCommand>
    {
        public CreateJobRequiredSubskillCommandValidator()
        {
            RuleFor(x => x.JobId).NotEmpty();
            RuleFor(x => x.SubskillId).NotEmpty();
        }
    }
}
