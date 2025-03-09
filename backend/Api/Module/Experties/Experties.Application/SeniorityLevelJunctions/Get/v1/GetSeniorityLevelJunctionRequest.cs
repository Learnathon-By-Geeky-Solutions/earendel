using MediatR;

namespace TalentMesh.Module.Experties.Application.SeniorityLevelJunctions.Get.v1;
public class GetSeniorityLevelJunctionRequest : IRequest<SeniorityLevelJunctionResponse>
{
    public Guid Id { get; set; }
    public GetSeniorityLevelJunctionRequest(Guid id) => Id = id;
}