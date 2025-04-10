using System;
using System.Collections.Generic;
using System.ComponentModel;
using MediatR;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Create.v1
{
    public sealed record CreateInterviewerAvailabilityCommand(
        Guid InterviewerId,
        List<AvailabilitySlot> AvailabilitySlots
    ) : IRequest<CreateInterviewerAvailabilityResponse>;

    public sealed record AvailabilitySlot(
        DateTime StartTime,
        DateTime EndTime
    );
}
