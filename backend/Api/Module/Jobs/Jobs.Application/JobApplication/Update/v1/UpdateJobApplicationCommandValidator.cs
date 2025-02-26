using FluentValidation;

namespace TalentMesh.Module.Job.Application.JobApplication.Update.v1
{
    public class UpdateJobApplicationCommandValidator : AbstractValidator<UpdateJobApplicationCommand>
    {
        public UpdateJobApplicationCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Application Id is required.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .MaximumLength(50).WithMessage("Status must not exceed 50 characters.");

            RuleFor(x => x.CoverLetter)
                .MaximumLength(1000).WithMessage("Cover Letter must not exceed 1000 characters.");
        }
    }
}
