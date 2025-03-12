using MediatR;
using System;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Update.v1
{
    public sealed record UpdateInterviewerApplicationCommand(
        Guid Id,
        Guid JobId,
        Guid InterviewerId,
        string Status,
        string? Comments = null
    ) : IRequest<UpdateInterviewerApplicationResponse>;
}
