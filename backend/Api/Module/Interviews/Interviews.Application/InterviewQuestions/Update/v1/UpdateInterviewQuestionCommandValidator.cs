using FluentValidation;

namespace TalentMesh.Module.Interviews.Application.InterviewQuestions.Update.v1;

public class UpdateInterviewQuestionCommandValidator : AbstractValidator<UpdateInterviewQuestionCommand>
{
    public UpdateInterviewQuestionCommandValidator()
    {
        // Validate Id (should not be empty)
        RuleFor(b => b.Id)
            .NotEmpty().WithMessage("Id is required.");

        // Validate ApplicationId (should not be empty)
        RuleFor(b => b.RubricId)
            .NotEmpty().WithMessage("RubricId is required.");

        RuleFor(b => b.InterviewId)
            .NotEmpty().WithMessage("InterviewId is required.");

        RuleFor(b => b.QuestionText)
            .MaximumLength(1000).WithMessage("QuestionText must not exceed 10000 characters.");
    }
}
