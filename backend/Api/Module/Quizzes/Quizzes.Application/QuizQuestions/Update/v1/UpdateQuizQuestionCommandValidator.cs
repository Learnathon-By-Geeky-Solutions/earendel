using FluentValidation;

namespace TalentMesh.Module.Quizzes.Application.QuizQuestions.Update.v1
{
    public class UpdateQuizQuestionCommandValidator : AbstractValidator<UpdateQuizQuestionCommand>
    {
        public UpdateQuizQuestionCommandValidator()
        {
            RuleFor(b => b.QuestionText)
                .NotEmpty().WithMessage("Question text is required for updating.")
                .MaximumLength(1000).WithMessage("Question text must not exceed 1000 characters.")
                .Matches(@"^[a-zA-Z0-9\s]+$").WithMessage("Question text can only contain alphanumeric characters and spaces.");
            
            RuleFor(b => b.CorrectOption)
                .InclusiveBetween(1, 4).WithMessage("Correct option must be between 1 and 4.")
                .NotNull().WithMessage("Correct option is required for updating the question.");

        }
    }
}
