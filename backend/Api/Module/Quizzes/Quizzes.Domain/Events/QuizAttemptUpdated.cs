using TalentMesh.Framework.Core.Domain.Events;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Quizzes.Domain.Events;
[ExcludeFromCodeCoverage]
public sealed record QuizAttemptUpdated : DomainEvent
{
    public QuizAttempt? QuizAttempt { get; set; }
}
