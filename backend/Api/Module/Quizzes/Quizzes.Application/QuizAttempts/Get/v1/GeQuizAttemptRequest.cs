using MediatR;

namespace TalentMesh.Module.Quizzes.Application.QuizAttempts.Get.v1;
public class GetQuizAttemptRequest : IRequest<QuizAttemptResponse>
{
    public Guid Id { get; set; }
    public GetQuizAttemptRequest(Guid id) => Id = id;
}
