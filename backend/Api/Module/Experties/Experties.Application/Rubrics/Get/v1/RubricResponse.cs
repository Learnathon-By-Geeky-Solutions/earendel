namespace TalentMesh.Module.Experties.Application.Rubrics.Get.v1;
public sealed record RubricResponse(Guid? Id, string Title, string? RubricDescription, Guid? SubSkillId, Guid? SeniorityId, decimal Weight);
