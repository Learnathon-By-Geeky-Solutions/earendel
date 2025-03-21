using System;
using System.ComponentModel;
using MediatR;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Create.v1
{
    public sealed record CreateInterviewerApplicationCommand(
        Guid JobId,
        Guid InterviewerId,
        [property: DefaultValue(null)] string? Comments = null
    ) : IRequest<CreateInterviewerApplicationResponse>;
}
