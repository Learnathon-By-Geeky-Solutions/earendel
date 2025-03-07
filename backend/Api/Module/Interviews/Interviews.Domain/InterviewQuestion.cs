using TalentMesh.Framework.Core.Domain;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Module.Interviews.Domain.Events;

namespace TalentMesh.Module.Interviews.Domain;
public class InterviewQuestion : AuditableEntity, IAggregateRoot
{
    public Guid RubricId { get; private set; }
    public Guid InterviewId { get; private set; }
    public string QuestionText { get; private set; } = null!;

    public virtual Interview Interview { get; private set; } = default!;

    public static InterviewQuestion Create(Guid rubricId, Guid interviewId, string questionText)
    {
        var interviewQuestion = new InterviewQuestion
        {
            RubricId = rubricId,
            InterviewId = interviewId,
            QuestionText = questionText
        };

        interviewQuestion.QueueDomainEvent(new InterviewQuestionCreated() { InterviewQuestion = interviewQuestion });

        return interviewQuestion;
    }

    public InterviewQuestion Update(Guid rubricId, Guid interviewId, string questionText)
    {
        if (RubricId != rubricId)
            RubricId = rubricId;

        if (InterviewId != interviewId)
            InterviewId = interviewId;

        if (!string.IsNullOrWhiteSpace(questionText) && !QuestionText.Equals(questionText, StringComparison.OrdinalIgnoreCase))
        {
            QuestionText = questionText;
            this.QueueDomainEvent(new InterviewQuestionUpdated() { InterviewQuestion = this });
        }

        return this;
    }
    public static InterviewQuestion Update(Guid id, Guid rubricId, Guid interviewId, string questionText)
    {
        var interviewQuestion = new InterviewQuestion
        {
            Id = id,
            RubricId = rubricId,
            InterviewId = interviewId,
            QuestionText = questionText
        };

        interviewQuestion.QueueDomainEvent(new InterviewQuestionUpdated() { InterviewQuestion = interviewQuestion });

        return interviewQuestion;
    }
}
