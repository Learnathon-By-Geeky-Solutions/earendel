using FluentValidation;

namespace TalentMesh.Module.Quizzes.Application.QuizAttemptAnswers.Update.v1;

public class UpdateQuizAttemptAnswerCommandValidator : AbstractValidator<UpdateQuizAttemptAnswerCommand>
{
    public UpdateQuizAttemptAnswerCommandValidator()
    {
        RuleFor(b => b.AttemptId)
            .NotEmpty().WithMessage("Attempt ID is required.");

        RuleFor(b => b.QuestionId)
            .NotEmpty().WithMessage("Question ID is required.");

        RuleFor(b => b.SelectedOption)
            .IsInEnum().WithMessage("Selected option must be a valid option.")
            .NotNull().WithMessage("Selected option is required.");

        RuleFor(b => b.IsCorrect)
            .NotNull().WithMessage("IsCorrect must be specified.");
    }
}
