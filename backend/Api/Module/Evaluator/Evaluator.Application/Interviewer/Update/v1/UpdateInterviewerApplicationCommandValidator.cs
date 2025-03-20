using FluentValidation;
using System.Diagnostics.CodeAnalysis;


namespace TalentMesh.Module.Evaluator.Application.Interviewer.Update.v1
{
    [ExcludeFromCodeCoverage]
    public class UpdateInterviewerApplicationCommandValidator : AbstractValidator<UpdateInterviewerApplicationCommand>
    {
        public UpdateInterviewerApplicationCommandValidator()
        {
            RuleFor(x => x.Status)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(50);
            RuleFor(x => x.Comments)
                .MaximumLength(1000);
        }
    }
}
