using TalentMesh.Framework.Core.Domain.Events;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Module.Quizzes.Domain.Events;
[ExcludeFromCodeCoverage]
public sealed record QuizQuestionCreated : DomainEvent
{
    public QuizQuestion? QuizQuestion { get; set; }
}
