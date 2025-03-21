using MediatR;
using System;

namespace TalentMesh.Module.Job.Application.JobRequiredSubskill.Delete.v1
{
    public sealed record DeleteJobRequiredSubskillCommand(Guid Id) : IRequest;
}
