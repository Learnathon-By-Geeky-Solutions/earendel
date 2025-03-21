using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Job.Application.JobRequiredSubskill.Update.v1
{
    [ExcludeFromCodeCoverage]
    public class UpdateJobRequiredSubskillCommandValidator : AbstractValidator<UpdateJobRequiredSubskillCommand>
    {
        public UpdateJobRequiredSubskillCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");
            RuleFor(x => x.JobId)
                .NotEmpty().WithMessage("Job Id is required.");
            RuleFor(x => x.SubskillId)
                .NotEmpty().WithMessage("Subskill Id is required.");
        }
    }
}
