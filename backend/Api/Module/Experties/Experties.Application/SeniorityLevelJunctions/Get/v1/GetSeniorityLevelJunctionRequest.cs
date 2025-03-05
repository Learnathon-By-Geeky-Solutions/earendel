using MediatR;

namespace TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Get.v1;
public class GetSeniorityLevelJunctionRequest : IRequest<SeniorityLevelJunctionResponse>
{
    public Guid Id { get; set; }
    public Guid SeniorityLevelId { get; set; }
    public Guid SkillId { get; set; }

    public GetSeniorityLevelJunctionRequest(Guid id, Guid seniorityLevelId, Guid skillId)
    {
        Id = id;
        SeniorityLevelId = seniorityLevelId;
        SkillId = skillId;
    }
}