using System;

namespace Evaluator.Application.Interviewer.Get.v1
{
    public sealed record InterviewerAvailabilityResponse(
        Guid? Id,
        Guid InterviewerId,
        DateTime StartTime,
        DateTime EndTime,
        bool IsAvailable
    );
}
