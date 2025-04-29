using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Update.v1;

[ExcludeFromCodeCoverage]
public class UpdateInterviewFeedbackCommandValidator : AbstractValidator<UpdateInterviewFeedbackCommand>
{
    public UpdateInterviewFeedbackCommandValidator()
    {
        RuleFor(b => b.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(b => b.InterviewId)
            .NotEmpty().WithMessage("InterviewId is required.");

        RuleFor(b => b.InterviewQuestionText)
            .NotEmpty().WithMessage("InterviewQuestionText is required.");

        RuleFor(b => b.Response)
            .NotEmpty().WithMessage("InterviewDate is required.");

        RuleFor(b => b.Score)
            .GreaterThanOrEqualTo(0).WithMessage("Score cannot be negative.");

    }
}
