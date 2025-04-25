namespace TalentMesh.Module.Interviews.Application.Interviews.Get.v1;

public sealed record InterviewResponse(
    Guid Id,            // Required, no need for nullable as it should always be present
    Guid ApplicationId, // Changed from UserId to ApplicationId
    Guid InterviewerId, // Added InterviewerId field
    Guid CandidateId, // Added CandidateId field
    Guid JobId, // Added JobId field
    DateTime InterviewDate, // Added InterviewDate
    string Status,       // Added Status field
    string Notes,        // Added Notes field
    string MeetingId     // Required, no need for nullable as it should always be present
);
