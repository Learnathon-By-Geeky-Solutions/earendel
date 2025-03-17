using System;

namespace Evaluator.Application.Interviewer.Get.v1
{
    public sealed record InterviewerApplicationResponse(
        Guid? Id,
        Guid JobId,
        Guid InterviewerId,
        DateTime AppliedDate,
        string Status,
        string? Comments
    );
}
