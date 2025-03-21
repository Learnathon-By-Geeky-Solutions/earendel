using FluentValidation;
using TalentMesh.Module.Evaluator.Application.Interviewer.Create.v1;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Create.v1
{
    [ExcludeFromCodeCoverage]
    public class CreateInterviewerEntryFormCommandValidator : AbstractValidator<CreateInterviewerEntryFormCommand>
    {
        public CreateInterviewerEntryFormCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId must be provided.");

            RuleFor(x => x.AdditionalInfo)
                .NotEmpty().WithMessage("Additional information is required.")
                .MinimumLength(10).WithMessage("Additional information must be at least 10 characters long.")
                .MaximumLength(2000).WithMessage("Additional information must not exceed 2000 characters.");
        }
    }
}
