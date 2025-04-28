using TalentMesh.Framework.Core.Domain;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Module.Interviews.Domain.Events;

namespace TalentMesh.Module.Interviews.Domain;
public class InterviewFeedback : AuditableEntity, IAggregateRoot
{
    public Guid InterviewId { get; private set; }
    // public Guid InterviewQuestionId { get; private set; }
    public string InterviewQuestionText { get; private set; } = null!;
    public string Response { get; private set; } = null!;
    public decimal Score { get; private set; }
    // public virtual InterviewQuestion InterviewQuestion { get; private set; } = default!;
    public virtual Interview Interview { get; private set; } = default!;

    public static InterviewFeedback Create(Guid interviewId, string interviewQuestionText, string response, decimal score)
    {
        var interviewFeedback = new InterviewFeedback
        {
            InterviewId = interviewId,
            // InterviewQuestionId = interviewQuestionId,
            InterviewQuestionText = interviewQuestionText,
            Response = response,
            Score = score
        };

        interviewFeedback.QueueDomainEvent(new InterviewFeedbackCreated() { InterviewFeedback = interviewFeedback });
        return interviewFeedback;
    }

    public InterviewFeedback Update(Guid interviewId, string interviewQuestionText, string response, decimal score)
    {
        if (!string.IsNullOrWhiteSpace(response) && !Response.Equals(response, StringComparison.OrdinalIgnoreCase))
        {
            Response = response;
        }

        if (!string.IsNullOrWhiteSpace(interviewQuestionText) && !Response.Equals(interviewQuestionText, StringComparison.OrdinalIgnoreCase))
        {
            InterviewQuestionText = interviewQuestionText;
        }

        if (InterviewId != interviewId)
            InterviewId = interviewId;

        // if (InterviewQuestionId != interviewQuestionId)
        //     InterviewQuestionId = interviewQuestionId;

        if (Score != score)
        {
            Score = score;
        }

        this.QueueDomainEvent(new InterviewFeedbackUpdated() { InterviewFeedback = this });
        return this;
    }

    public static InterviewFeedback Update(Guid id, Guid interviewId, string interviewQuestionText, string response, decimal score)
    {
        var interviewFeedback = new InterviewFeedback
        {
            Id = id,
            InterviewId = interviewId,
            // InterviewQuestionId = interviewQuestionId,
            InterviewQuestionText = interviewQuestionText,
            Response = response,
            Score = score
        };

        interviewFeedback.QueueDomainEvent(new InterviewFeedbackUpdated() { InterviewFeedback = interviewFeedback });
        return interviewFeedback;
    }
}