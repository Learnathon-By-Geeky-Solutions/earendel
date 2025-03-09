using FluentValidation;

namespace TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Update.v1;

public class UpdateInterviewFeedbackCommandValidator : AbstractValidator<UpdateInterviewFeedbackCommand>
{
    public UpdateInterviewFeedbackCommandValidator()
    {
        RuleFor(b => b.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(b => b.InterviewId)
            .NotEmpty().WithMessage("ApplicationId is required.");

        RuleFor(b => b.InterviewQuestionId)
            .NotEmpty().WithMessage("InterviewerId is required.");

        RuleFor(b => b.Response)
            .NotEmpty().WithMessage("InterviewDate is required.");

        RuleFor(b => b.Score)
            .GreaterThanOrEqualTo(0).WithMessage("Score cannot be negative.");

    }
}
