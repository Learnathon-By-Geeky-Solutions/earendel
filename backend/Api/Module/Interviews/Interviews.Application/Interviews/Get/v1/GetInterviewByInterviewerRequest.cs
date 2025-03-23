using MediatR;

namespace TalentMesh.Module.Interviews.Application.Interviews.Get.v1;
public class GetInterviewByInterviewerRequest : IRequest<bool>
{
    public Guid Id { get; set; }
    public GetInterviewByInterviewerRequest(Guid id) => Id = id;
}
