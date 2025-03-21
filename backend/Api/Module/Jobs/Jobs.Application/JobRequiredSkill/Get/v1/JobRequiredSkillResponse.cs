using System;

namespace TalentMesh.Module.Job.Application.JobRequiredSkill.Get.v1
{
    public sealed record JobRequiredSkillResponse(
        Guid? Id,
        Guid JobId,
        Guid SkillId
    );
}
