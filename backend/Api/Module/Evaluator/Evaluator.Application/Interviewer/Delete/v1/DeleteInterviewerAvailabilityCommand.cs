using MediatR;
using System;

namespace TalentMesh.Module.Evaluator.Application.Interviewer.Delete.v1
{
    public sealed record DeleteInterviewerAvailabilityCommand(Guid Id) : IRequest;
}
