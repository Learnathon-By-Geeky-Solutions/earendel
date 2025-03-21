using MediatR;
using System;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Update.v1
{
    public sealed record UpdateInterviewerAvailabilityCommand(
        Guid Id,
        Guid InterviewerId,
        DateTime StartTime,
        DateTime EndTime,
        bool IsAvailable
    ) : IRequest<UpdateInterviewerAvailabilityResponse>;
}
