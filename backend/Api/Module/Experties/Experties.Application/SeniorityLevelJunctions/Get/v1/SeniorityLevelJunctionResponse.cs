using TalentMesh.Module.Experties.Application.Seniorities.Get.v1;

namespace TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Get.v1;
public sealed record SeniorityLevelJunctionResponse(
    Guid Id,
    Guid SeniorityLevelId,
    Guid SkillId, 
    SeniorityResponse Seniority);
