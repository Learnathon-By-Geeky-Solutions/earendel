using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Create.v1;

[ExcludeFromCodeCoverage]

public class CreateInterviewFeedbackCommandValidator : AbstractValidator<CreateInterviewFeedbackCommand>
{
    public CreateInterviewFeedbackCommandValidator()
    {
        RuleFor(b => b.InterviewId)
            .NotEmpty().WithMessage("InterviewId is required.");

        RuleFor(b => b.InterviewQuestionText)
            .NotEmpty().WithMessage("InterviewQuestionText is required.");

        RuleFor(b => b.Response)
            .NotEmpty().WithMessage("Response is required.")
            .MaximumLength(1000).WithMessage("Response must not exceed 1000 characters.");

        RuleFor(b => b.Score)
            .InclusiveBetween(0, 5).WithMessage("Score must be between 0 and 5.");
    }
}
