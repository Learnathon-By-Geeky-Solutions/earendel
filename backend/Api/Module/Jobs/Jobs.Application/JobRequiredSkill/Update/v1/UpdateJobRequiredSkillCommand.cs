using MediatR;
using System;

namespace TalentMesh.Module.Job.Application.JobRequiredSkill.Update.v1
{
    public sealed record UpdateJobRequiredSkillCommand(
        Guid Id,
        Guid JobId,
        Guid SkillId
    ) : IRequest<UpdateJobRequiredSkillResponse>;
}
