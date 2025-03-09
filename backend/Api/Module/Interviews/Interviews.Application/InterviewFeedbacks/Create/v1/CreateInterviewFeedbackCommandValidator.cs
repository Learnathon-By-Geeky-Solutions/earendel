using FluentValidation;

namespace TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Create.v1;

public class CreateInterviewFeedbackCommandValidator : AbstractValidator<CreateInterviewFeedbackCommand>
{
    public CreateInterviewFeedbackCommandValidator()
    {
        RuleFor(b => b.InterviewId)
            .NotEmpty().WithMessage("InterviewId is required.");

        RuleFor(b => b.InterviewQuestionId)
            .NotEmpty().WithMessage("InterviewQuestionId is required.");

        RuleFor(b => b.Response)
            .NotEmpty().WithMessage("Response is required.")
            .MaximumLength(1000).WithMessage("Response must not exceed 1000 characters.");

        RuleFor(b => b.Score)
            .InclusiveBetween(0, 10).WithMessage("Score must be between 0 and 10.");
    }
}
