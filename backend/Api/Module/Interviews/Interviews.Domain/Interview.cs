using TalentMesh.Framework.Core.Domain;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Module.Interviews.Domain.Events;

namespace TalentMesh.Module.Interviews.Domain;
public class Interview : AuditableEntity, IAggregateRoot
{
    public Guid ApplicationId { get; private set; }
    public Guid InterviewerId { get; private set; }
    public Guid CandidateId { get; private set; }
    public Guid JobId { get; private set; }
    public DateTime InterviewDate { get; private set; }
    public string Status { get; private set; } = null!;
    public string Notes { get; private set; } = null!;
    public string MeetingId { get; private set; } = null!; // New field added

    // Updated static Create method to include MeetingId
    public static Interview Create(InterviewDetails details)
    {
        var interview = new Interview
        {
            ApplicationId = details.ApplicationId,
            InterviewerId = details.InterviewerId,
            CandidateId = details.CandidateId,
            JobId = details.JobId,
            InterviewDate = details.InterviewDate,
            Status = details.Status,
            Notes = details.Notes ?? string.Empty,
            MeetingId = details.MeetingId
        };

        interview.QueueDomainEvent(new InterviewCreated() { Interview = interview });

        return interview;
    }

    // Updated Update method to include MeetingId
    public Interview Update(InterviewDetails details)
    {
        if (ApplicationId != details.ApplicationId)
            ApplicationId = details.ApplicationId;
        if (InterviewerId != details.InterviewerId)
            InterviewerId = details.InterviewerId;
        if (CandidateId != details.CandidateId)
            CandidateId = details.CandidateId;
        if (JobId != details.JobId)
            JobId = details.JobId;
        if (InterviewDate != details.InterviewDate)
            InterviewDate = details.InterviewDate;
        if (!string.IsNullOrWhiteSpace(details.Status) && !Status.Equals(details.Status, StringComparison.OrdinalIgnoreCase))
            Status = details.Status;
        if (!string.IsNullOrWhiteSpace(details.Notes) && !Notes.Equals(details.Notes, StringComparison.OrdinalIgnoreCase))
            Notes = details.Notes;
        if (!string.IsNullOrWhiteSpace(details.MeetingId) && !MeetingId.Equals(details.MeetingId, StringComparison.OrdinalIgnoreCase))
            MeetingId = details.MeetingId;

        this.QueueDomainEvent(new InterviewUpdated() { Interview = this });

        return this;
    }

    // Updated static Update method to include MeetingId
    public static Interview Reconstruct(Guid id, InterviewDetails details)
    {
        var interview = new Interview
        {
            Id = id,
            ApplicationId = details.ApplicationId,
            InterviewerId = details.InterviewerId,
            CandidateId = details.CandidateId,
            JobId = details.JobId,
            InterviewDate = details.InterviewDate,
            Status = details.Status,
            Notes = details.Notes ?? string.Empty,
            MeetingId = details.MeetingId ?? string.Empty
        };
        interview.QueueDomainEvent(new InterviewUpdated() { Interview = interview });
        return interview;
    }

}
