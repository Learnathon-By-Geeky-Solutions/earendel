using TalentMesh.Framework.Core.Exceptions;

namespace TalentMesh.Module.Quizzes.Domain.Exceptions;
public sealed class QuizQuestionNotFoundException : NotFoundException
{
    public QuizQuestionNotFoundException(Guid id)
        : base($"QuizQuestion with id {id} not found")
    {
    }
}
