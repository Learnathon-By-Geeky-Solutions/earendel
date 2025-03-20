using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Quizzes.Application.QuizAttempts.Create.v1;

[ExcludeFromCodeCoverage]

public class CreateQuizAttemptCommandValidator : AbstractValidator<CreateQuizAttemptCommand>
{
    public CreateQuizAttemptCommandValidator()
    {
        RuleFor(b => b.TotalQuestions)
            .GreaterThanOrEqualTo(1).WithMessage("TotalQuestions must be at least 1.");

        RuleFor(b => b.Score)
            .GreaterThanOrEqualTo(0).WithMessage("Score cannot be negative.");
    }
}
