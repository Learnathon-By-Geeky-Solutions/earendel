using FluentValidation;

namespace TalentMesh.Module.Interviews.Application.InterviewQuestions.Create.v1;

public class CreateInterviewQuestionCommandValidator : AbstractValidator<CreateInterviewQuestionCommand>
{
    public CreateInterviewQuestionCommandValidator()
    {
        RuleFor(b => b.RubricId)
            .NotEmpty().WithMessage("RubricId is required.");

        RuleFor(b => b.QuestionText)
            .NotEmpty().WithMessage("QuestionText is required.")
            .MaximumLength(500).WithMessage("QuestionText must not exceed 500 characters.");
    }
}
