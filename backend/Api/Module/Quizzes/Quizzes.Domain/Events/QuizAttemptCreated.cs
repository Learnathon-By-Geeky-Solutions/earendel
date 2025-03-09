using TalentMesh.Framework.Core.Domain.Events;

namespace TalentMesh.Module.Quizzes.Domain.Events;
public sealed record QuizAttemptCreated : DomainEvent
{
    public QuizAttempt? QuizAttempt { get; set; }
}
