using MediatR;

namespace TalentMesh.Module.Interviews.Application.Interviews.Get.v1;
public class GetInterviewRequest : IRequest<InterviewResponse>
{
    public Guid Id { get; set; }
    public GetInterviewRequest(Guid id) => Id = id;
}
