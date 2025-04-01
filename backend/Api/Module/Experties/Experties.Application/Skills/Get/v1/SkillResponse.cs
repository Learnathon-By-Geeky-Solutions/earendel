using TalentMesh.Module.Experties.Application.SubSkills.Get.v1;
using TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Get.v1;

namespace TalentMesh.Module.Experties.Application.Skills.Get.v1;
public sealed record SkillResponse(Guid? Id, string Name, string? Description, List<SubSkillResponse> SubSkills,
        List<SeniorityLevelJunctionResponse> SeniorityLevelJunctions
    );

