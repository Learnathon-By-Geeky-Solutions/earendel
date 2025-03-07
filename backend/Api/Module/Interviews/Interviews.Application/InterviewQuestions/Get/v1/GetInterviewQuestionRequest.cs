using MediatR;

namespace TalentMesh.Module.Interviews.Application.InterviewQuestions.Get.v1;
public class GetInterviewQuestionRequest : IRequest<InterviewQuestionResponse>
{
    public Guid Id { get; set; }
    public GetInterviewQuestionRequest(Guid id) => Id = id;
}
