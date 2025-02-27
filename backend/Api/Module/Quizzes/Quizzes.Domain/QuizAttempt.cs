using System.Net.Http.Headers;
using TalentMesh.Framework.Core.Domain;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Module.Quizzes.Domain.Events;


namespace TalentMesh.Module.Quizzes.Domain;
public class QuizAttempt : AuditableEntity, IAggregateRoot
{
    public Guid UserId { get; private set; }
    public decimal Score { get; private set; }
    public int TotalQuestions { get; private set; }

    public static QuizAttempt Create(Guid userId, decimal score, int totalQuestions)
    {
        var QuizAttempt = new QuizAttempt
        {
            UserId = userId,
            Score = score,
            TotalQuestions = totalQuestions
        };

        QuizAttempt.QueueDomainEvent(new QuizAttemptCreated() { QuizAttempt = QuizAttempt });

        return QuizAttempt;
    }

    public QuizAttempt Update(Guid? userId, decimal? score, int totalQuestions)
    {
        if (userId.HasValue && userId.Value != Guid.Empty && userId != userId.Value)
        {
            UserId = userId.Value;
        }

        if (score.HasValue && score.Value > 0 && score != score.Value)
            Score = score.Value;
        if (totalQuestions > 0)
            TotalQuestions = totalQuestions;
        
        this.QueueDomainEvent(new QuizAttemptUpdated() { QuizAttempt = this });

        return this;
    }

    public static QuizAttempt Update(Guid id, Guid userId, decimal score, int totalQuestions)
    {
        var QuizAttempt = new QuizAttempt
        {
            Id = id,
            UserId = userId,
            Score = score,
            TotalQuestions = totalQuestions
        };

        QuizAttempt.QueueDomainEvent(new QuizAttemptUpdated() { QuizAttempt = QuizAttempt });

        return QuizAttempt;
    }
}

