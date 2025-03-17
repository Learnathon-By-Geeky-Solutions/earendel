using MediatR;
using System;

namespace TalentMesh.Module.Job.Application.JobRequiredSkill.Delete.v1
{
    public sealed record DeleteJobRequiredSkillCommand(Guid Id) : IRequest;
}
