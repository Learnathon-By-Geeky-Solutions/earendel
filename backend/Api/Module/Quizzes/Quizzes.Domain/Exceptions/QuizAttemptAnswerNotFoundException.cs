using TalentMesh.Framework.Core.Exceptions;

namespace TalentMesh.Module.Quizzes.Domain.Exceptions;
public sealed class QuizAttemptAnswerNotFoundException : NotFoundException
{
    public QuizAttemptAnswerNotFoundException(Guid id)
        : base($"QuizAttemptAnswer with id {id} not found")
    {
    }
}
