using MediatR;

namespace TalentMesh.Module.Experties.Application.Rubrics.Get.v1;
public class GetRubricRequest : IRequest<RubricResponse>
{
    public Guid Id { get; set; }
    public GetRubricRequest(Guid id) => Id = id;
}
