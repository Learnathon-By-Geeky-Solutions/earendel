using TalentMesh.Framework.Core.Exceptions;

namespace TalentMesh.Module.Quizzes.Domain.Exceptions;
public sealed class QuizAttemptNotFoundException : NotFoundException
{
    public QuizAttemptNotFoundException(Guid id)
        : base($"QuizAttempt with id {id} not found")
    {
    }
}
