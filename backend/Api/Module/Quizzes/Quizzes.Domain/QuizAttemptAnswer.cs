using TalentMesh.Framework.Core.Domain;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Module.Quizzes.Domain.Events;

namespace TalentMesh.Module.Quizzes.Domain;
public class QuizAttemptAnswer : AuditableEntity, IAggregateRoot
{
    public Guid AttemptId { get; private set; }
    public Guid QuestionId { get; private set; }
    public int SelectedOption { get; private set; }
    public bool IsCorrect { get; private set; }

    public virtual QuizAttempt QuizAttempt { get; private set; } = default!;
    public virtual QuizQuestion QuizQuestion { get; private set; } = default!;

    // Create a new QuizAttemptAnswer
    public static QuizAttemptAnswer Create(Guid attemptId, Guid questionId, int selectedOption, bool isCorrect)
    {
        var quizAttemptAnswer = new QuizAttemptAnswer
        {
            AttemptId = attemptId,
            QuestionId = questionId,
            SelectedOption = selectedOption,
            IsCorrect = isCorrect,
        };

        quizAttemptAnswer.QueueDomainEvent(new QuizAttemptAnswerCreated { QuizAttemptAnswer = quizAttemptAnswer });

        return quizAttemptAnswer;
    }

    // Update the QuizAttemptAnswer
    public QuizAttemptAnswer Update(Guid? attemptId, Guid? questionId, int? selectedOption, bool? isCorrect)
    {
        if (attemptId.HasValue && attemptId.Value is not null && attemptId != AttemptId)
            AttemptId = attemptId.Value;

        if (questionId.HasValue && questionId != QuestionId)
            QuestionId = questionId.Value;

        if (selectedOption.HasValue && selectedOption != SelectedOption)
            SelectedOption = selectedOption.Value;

        if (isCorrect.HasValue && isCorrect != IsCorrect)
            IsCorrect = isCorrect.Value;

        QueueDomainEvent(new QuizAttemptAnswerUpdated { QuizAttemptAnswer = this });

        return this;
    }

    // Another version of Update method to set all fields at once
    public static QuizAttemptAnswer Update(Guid id, Guid attemptId, Guid questionId, int selectedOption, bool isCorrect)
    {
        var quizAttemptAnswer = new QuizAttemptAnswer
        {
            Id = id,
            AttemptId = attemptId,
            QuestionId = questionId,
            SelectedOption = selectedOption,
            IsCorrect = isCorrect,
        };

        quizAttemptAnswer.QueueDomainEvent(new QuizAttemptAnswerUpdated { QuizAttemptAnswer = quizAttemptAnswer });

        return quizAttemptAnswer;
    }
}
