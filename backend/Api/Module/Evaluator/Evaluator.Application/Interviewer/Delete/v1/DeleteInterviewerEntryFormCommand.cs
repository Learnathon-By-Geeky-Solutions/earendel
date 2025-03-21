using MediatR;
using System;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Delete.v1
{
    public sealed record DeleteInterviewerEntryFormCommand(Guid Id) : IRequest;
}
