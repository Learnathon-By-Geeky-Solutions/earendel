using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Create.v1
{
    [ExcludeFromCodeCoverage]
    public class CreateInterviewerAvailabilityCommandValidator : AbstractValidator<CreateInterviewerAvailabilityCommand>
    {
        public CreateInterviewerAvailabilityCommandValidator()
        {
            RuleFor(x => x.InterviewerId)
                .NotEmpty().WithMessage("InterviewerId must be provided.");

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("StartTime must be provided.");

            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("EndTime must be provided.")
                .GreaterThan(x => x.StartTime)
                .WithMessage("EndTime must be after StartTime.");
        }
    }
}
