using MediatR;
using System;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Update.v1
{
    public sealed record UpdateInterviewerEntryFormCommand(
        Guid Id,
        Guid UserId,
        string? AdditionalInfo,
        string Status
    ) : IRequest<UpdateInterviewerEntryFormResponse>;
}
