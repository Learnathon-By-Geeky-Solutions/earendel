using MediatR;

namespace TalentMesh.Module.Experties.Application.Seniorities.Get.v1;
public class GetSeniorityRequest : IRequest<SeniorityResponse>
{
    public Guid Id { get; set; }
    public GetSeniorityRequest(Guid id) => Id = id;
}
