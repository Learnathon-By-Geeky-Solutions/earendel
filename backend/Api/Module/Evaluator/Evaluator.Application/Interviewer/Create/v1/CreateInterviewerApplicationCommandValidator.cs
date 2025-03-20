using FluentValidation;
using TalentMesh.Module.Evaluator.Application.Interviewer.Create.v1;
using System.Diagnostics.CodeAnalysis;


namespace TalentMesh.Module.Evaluator.Application.Interviewer.Create.v1
{
    [ExcludeFromCodeCoverage]
    public class CreateInterviewerApplicationCommandValidator : AbstractValidator<CreateInterviewerApplicationCommand>
    {
        public CreateInterviewerApplicationCommandValidator()
        {
            RuleFor(x => x.JobId)
                .NotEmpty().WithMessage("JobId must be provided.");

            RuleFor(x => x.InterviewerId)
                .NotEmpty().WithMessage("InterviewerId must be provided.");

            RuleFor(x => x.Comments)
                .MaximumLength(1000).WithMessage("Comments must not exceed 1000 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Comments));
        }
    }
}
