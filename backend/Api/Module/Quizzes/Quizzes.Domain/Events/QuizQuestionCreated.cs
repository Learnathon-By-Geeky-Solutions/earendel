using TalentMesh.Framework.Core.Domain.Events;

namespace TalentMesh.Module.Quizzes.Domain.Events;
public sealed record QuizQuestionCreated : DomainEvent
{
    public QuizQuestion? QuizQuestion { get; set; }
}
