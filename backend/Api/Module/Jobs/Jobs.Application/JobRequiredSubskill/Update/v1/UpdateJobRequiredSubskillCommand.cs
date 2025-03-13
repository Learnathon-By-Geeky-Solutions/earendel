using MediatR;
using System;

namespace TalentMesh.Module.Job.Application.JobRequiredSubskill.Update.v1
{
    public sealed record UpdateJobRequiredSubskillCommand(
        Guid Id,
        Guid JobId,
        Guid SubskillId
    ) : IRequest<UpdateJobRequiredSubskillResponse>;
}
