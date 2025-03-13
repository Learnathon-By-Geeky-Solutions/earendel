using System;
using System.ComponentModel;
using MediatR;

namespace TalentMesh.Module.Job.Application.JobRequiredSkill.Create.v1
{
    public sealed record CreateJobRequiredSkillCommand(
        Guid JobId,
        Guid SkillId
    ) : IRequest<CreateJobRequiredSkillResponse>;
}
