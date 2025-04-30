using System;
using System.Diagnostics.CodeAnalysis;

namespace Evaluator.Application.Interviewer.Get.v1
{
    [ExcludeFromCodeCoverage]
    public sealed record InterviewerEntryFormResponse(
        Guid? Id,
        Guid UserId,
        string? CV,
        string? AdditionalInfo,
        string Status,
        DateTimeOffset Created
    );
}
