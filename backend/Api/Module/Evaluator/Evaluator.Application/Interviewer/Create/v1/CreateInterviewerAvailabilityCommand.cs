using System;
using System.ComponentModel;
using MediatR;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Create.v1
{
    public sealed record CreateInterviewerAvailabilityCommand(
        Guid InterviewerId,
        DateTime StartTime,
        DateTime EndTime,
        [property: DefaultValue(true)] bool IsAvailable = true
    ) : IRequest<CreateInterviewerAvailabilityResponse>;
}
