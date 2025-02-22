using MediatR;

namespace TalentMesh.Module.Experties.Application.SubSkills.Get.v1;
public class GetSubSkillRequest : IRequest<SubSkillResponse>
{
    public Guid Id { get; set; }
    public GetSubSkillRequest(Guid id) => Id = id;
}
