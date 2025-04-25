using System;
using System.ComponentModel;
using MediatR;

namespace TalentMesh.Module.Interviews.Application.Interviews.Create.v1;

public sealed record CreateInterviewCommand(
    Guid ApplicationId,
    Guid InterviewerId,
    Guid CandidateId,
    Guid JobId,
    DateTime InterviewDate,
    string Status,
    [property: DefaultValue(null)] string Notes,
    string MeetingId // New field added
) : IRequest<CreateInterviewResponse>;
