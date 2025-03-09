using FluentValidation;

namespace TalentMesh.Module.Quizzes.Application.QuizAttemptAnswers.Create.v1;

public class CreateQuizAttemptAnswerCommandValidator : AbstractValidator<CreateQuizAttemptAnswerCommand>
{
    public CreateQuizAttemptAnswerCommandValidator()
    {
        RuleFor(b => b.AttemptId)
            .NotEmpty().WithMessage("Attempt ID is required.");

        RuleFor(b => b.QuestionId)
            .NotEmpty().WithMessage("Question ID is required.");

        RuleFor(b => b.SelectedOption)
            .IsInEnum().WithMessage("Selected option must be between 1 and 4.");

        RuleFor(b => b.IsCorrect)
            .NotNull().WithMessage("IsCorrect must be specified.");
    }
}
