using System;

namespace TalentMesh.Module.Job.Application.JobRequiredSubskill.Get.v1
{
    public sealed record JobRequiredSubskillResponse(
        Guid? Id,
        Guid JobId,
        Guid SubskillId
    );
}
