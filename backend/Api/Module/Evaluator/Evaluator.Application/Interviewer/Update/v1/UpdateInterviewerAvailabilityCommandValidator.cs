using FluentValidation;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Update.v1
{
    public class UpdateInterviewerAvailabilityCommandValidator : AbstractValidator<UpdateInterviewerAvailabilityCommand>
    {
        public UpdateInterviewerAvailabilityCommandValidator()
        {
            RuleFor(x => x.StartTime)
                .LessThan(x => x.EndTime)
                .WithMessage("StartTime must be before EndTime.");
        }
    }
}
