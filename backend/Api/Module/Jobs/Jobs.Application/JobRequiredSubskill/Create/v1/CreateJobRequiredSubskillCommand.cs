using System;
using System.ComponentModel;
using MediatR;

namespace TalentMesh.Module.Job.Application.JobRequiredSubskill.Create.v1
{
    public sealed record CreateJobRequiredSubskillCommand(
        Guid JobId,
        Guid SubskillId
    ) : IRequest<CreateJobRequiredSubskillResponse>;
}
