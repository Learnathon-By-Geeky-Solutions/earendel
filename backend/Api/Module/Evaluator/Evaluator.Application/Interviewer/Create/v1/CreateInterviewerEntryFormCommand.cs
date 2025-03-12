using System;
using System.ComponentModel;
using MediatR;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Create.v1
{
    public sealed record CreateInterviewerEntryFormCommand(
        Guid UserId,
        [property: DefaultValue("Please provide additional details such as resume, experience, etc.")] string AdditionalInfo
    ) : IRequest<CreateInterviewerEntryFormResponse>;
}
