using MediatR;

namespace TalentMesh.Module.Interviews.Application.Interviews.Update.v1;

public sealed record UpdateInterviewCommand(
    Guid Id,
    Guid ApplicationId,     // Interview related field
    Guid InterviewerId,     // Interview related field
    Guid CandidateId,     // Interview related field
    Guid JobId,     // Interview related field
    DateTime InterviewDate, // Interview related field
    string Status,          // Interview related field
    string? Notes,          // Interview related field, optional
    string MeetingId        // Meeting ID, required
) : IRequest<UpdateInterviewResponse>;
