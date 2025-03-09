using TalentMesh.Framework.Core.Domain;
using TalentMesh.Framework.Core.Domain.Contracts;
using TalentMesh.Module.Interviews.Domain.Events;

namespace TalentMesh.Module.Interviews.Domain;
public class Interview : AuditableEntity, IAggregateRoot
{
    public Guid ApplicationId { get; private set; }
    public Guid InterviewerId { get; private set; }
    public DateTime InterviewDate { get; private set; }
    public string Status { get; private set; } = null!;
    public string Notes { get; private set; } = null!;
    public string MeetingId { get; private set; } = null!; // New field added

    // Updated static Create method to include MeetingId
    public static Interview Create(Guid applicationId, Guid interviewerId, DateTime interviewDate, string status, string? notes, string meetingId)
    {
        var interview = new Interview
        {
            ApplicationId = applicationId,
            InterviewerId = interviewerId,
            InterviewDate = interviewDate,
            Status = status,
            Notes = notes ?? string.Empty,
            MeetingId = meetingId // Set MeetingId
        };

        interview.QueueDomainEvent(new InterviewCreated() { Interview = interview });

        return interview;
    }

    // Updated Update method to include MeetingId
    public Interview Update(Guid applicationId, Guid interviewerId, DateTime interviewDate, string status, string? notes, string? meetingId)
    {
        if (ApplicationId != applicationId)
            ApplicationId = applicationId;

        if (InterviewerId != interviewerId)
            InterviewerId = interviewerId;

        if (InterviewDate != interviewDate)
            InterviewDate = interviewDate;

        if (!string.IsNullOrWhiteSpace(status) && !Status.Equals(status, StringComparison.OrdinalIgnoreCase))
            Status = status;

        if (!string.IsNullOrWhiteSpace(notes) && !Notes.Equals(notes, StringComparison.OrdinalIgnoreCase))
            Notes = notes;

        // Update MeetingId if provided and not the same
        if (!string.IsNullOrWhiteSpace(meetingId) && !MeetingId.Equals(meetingId, StringComparison.OrdinalIgnoreCase))
            MeetingId = meetingId;

        this.QueueDomainEvent(new InterviewUpdated() { Interview = this });

        return this;
    }

    // Updated static Update method to include MeetingId
    public static Interview Update(Guid id, Guid applicationId, Guid interviewerId, DateTime interviewDate, string status, string? notes, string? meetingId)
    {
        var interview = new Interview
        {
            Id = id,
            ApplicationId = applicationId,
            InterviewerId = interviewerId,
            InterviewDate = interviewDate,
            Status = status,
            Notes = notes ?? string.Empty,
            MeetingId = meetingId ?? string.Empty // Set MeetingId
        };

        interview.QueueDomainEvent(new InterviewUpdated() { Interview = interview });

        return interview;
    }
}
