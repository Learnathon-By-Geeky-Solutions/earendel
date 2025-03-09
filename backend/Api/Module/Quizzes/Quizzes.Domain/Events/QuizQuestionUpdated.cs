using TalentMesh.Framework.Core.Domain.Events;

namespace TalentMesh.Module.Quizzes.Domain.Events;
public sealed record QuizQuestionUpdated : DomainEvent
{
    public QuizQuestion? QuizQuestion { get; set; }
}
