using MediatR;

namespace TalentMesh.Module.Interviews.Application.InterviewFeedbacks.Get.v1;
public class GetInterviewFeedbackRequest : IRequest<InterviewFeedbackResponse>
{
    public Guid Id { get; set; }
    public GetInterviewFeedbackRequest(Guid id) => Id = id;
}
