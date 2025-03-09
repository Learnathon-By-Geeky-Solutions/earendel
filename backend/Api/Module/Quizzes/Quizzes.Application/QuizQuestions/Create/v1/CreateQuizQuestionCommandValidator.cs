using FluentValidation;

namespace TalentMesh.Module.Quizzes.Application.QuizQuestions.Create.v1
{
    public class CreateQuizQuestionCommandValidator : AbstractValidator<CreateQuizQuestionCommand>
    {
        public CreateQuizQuestionCommandValidator()
        {
            RuleFor(b => b.QuestionText)
                .NotEmpty().WithMessage("Question text is required.")
                .MaximumLength(1000).WithMessage("Question text must not exceed 1000 characters.")
                .Matches(@"^[a-zA-Z0-9\s]+$").WithMessage("Question text can only contain alphanumeric characters and spaces.");

            RuleFor(b => b.Option1)
                .NotEmpty().WithMessage("Option 1 is required.")
                .MaximumLength(255).WithMessage("Option 1 must not exceed 255 characters.");

            RuleFor(b => b.Option2)
                .NotEmpty().WithMessage("Option 2 is required.")
                .MaximumLength(255).WithMessage("Option 2 must not exceed 255 characters.");

            RuleFor(b => b.Option3)
                .NotEmpty().WithMessage("Option 3 is required.")
                .MaximumLength(255).WithMessage("Option 3 must not exceed 255 characters.");

            RuleFor(b => b.Option4)
                .NotEmpty().WithMessage("Option 4 is required.")
                .MaximumLength(255).WithMessage("Option 4 must not exceed 255 characters.");

            RuleFor(b => b.CorrectOption)
                .InclusiveBetween(1, 4).WithMessage("Correct option must be between 1 and 4.")
                .NotNull().WithMessage("Correct option is required for creating the question.");

            RuleFor(b => b)
                .Must(b => b.Option1 != b.Option2 && b.Option1 != b.Option3 && b.Option1 != b.Option4)
                .WithMessage("Option 1 must be unique and not the same as Option 2, Option 3, or Option 4.");
        }
    }
}
