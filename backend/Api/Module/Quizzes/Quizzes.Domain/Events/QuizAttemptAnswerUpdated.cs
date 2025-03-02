using TalentMesh.Framework.Core.Domain.Events;

namespace TalentMesh.Module.Quizzes.Domain.Events;
public sealed record QuizAttemptAnswerUpdated : DomainEvent
{
    public QuizAttemptAnswer? QuizAttemptAnswer { get; set; }
}
