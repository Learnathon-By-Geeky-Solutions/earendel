using FluentValidation;

namespace TalentMesh.Module.Quizzes.Application.QuizAttempts.Update.v1;
public class UpdateQuizAttemptCommandValidator : AbstractValidator<UpdateQuizAttemptCommand>
{
    public UpdateQuizAttemptCommandValidator()
    {
        RuleFor(b => b.TotalQuestions)
            .GreaterThanOrEqualTo(1).WithMessage("TotalQuestions must be at least 1.");

        RuleFor(b => b.Score)
            .GreaterThanOrEqualTo(0).WithMessage("Score cannot be negative.");
    }

}


